using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDITranslation
{
    public enum EDIFileType
    {
        OACY, UNSC, RURD, SWNT, NMQR, SQTS, Ack, Unknown, SQTSOP
    }

    public enum EDIEnvelopeNodes
    {
        ISA, GS, ST, IEA, GE, SE
    }

    public class EDIWrapperBase
    {
        protected string _ediFile;
        protected char[] _segmentSeparator = new char[] { '~' };
        protected char[] _dataSeparator = new char['*'];
        protected string[] _segments;
        protected List<string[]> _stEnvelopes = new List<string[]>();
        private EDIFileType _ediFileType;

        public EDIFileType FileType
        { get { return _ediFileType; } }

        public EDIWrapperBase(string ediFile, char[] segmentSeparator, char[] dataSeparator)
        {
            _ediFile = ediFile;
            _segmentSeparator = segmentSeparator;
            _dataSeparator = dataSeparator;

            _segments = _ediFile.Trim().Split(_segmentSeparator);
            _stEnvelopes = EnvelopeBlocks(EDIEnvelopeNodes.ST, EDIEnvelopeNodes.SE);
            _ediFileType = ReturnEDIFileType();
        }


        public virtual List<string[]> EnvelopeBlocks(EDIEnvelopeNodes startingNode, EDIEnvelopeNodes endingNode)
        {
            List<string[]> envelopeBlocks = new List<string[]>();
            Hashtable blockIndexes = new Hashtable();
            int matchingValueIndexForISA = 13;
            int matchingValueIndexForGS = 6;
            int matchingValueIndexForST = 2;

            var startNodeIndexes = Enumerable.Range(0, _segments.Count())
                 .Where(i => _segments[i].StartsWith(startingNode.ToString()))
                 .ToList();

            var endingNodeIdexes = Enumerable.Range(0, _segments.Count())
                .Where(i => _segments[i].StartsWith(endingNode.ToString()))
                .ToList();

            foreach (var startNodeIndex in startNodeIndexes)
            {
                string[] startNode = _segments[startNodeIndex].Split(_dataSeparator);
                string matchingValue = "-1";

                switch (startingNode)
                {
                    case EDIEnvelopeNodes.ISA:
                        matchingValue = startNode[matchingValueIndexForISA];
                        break;
                    case EDIEnvelopeNodes.ST:
                        matchingValue = startNode[matchingValueIndexForST];
                        break;
                    case EDIEnvelopeNodes.GS:
                        matchingValue = startNode[matchingValueIndexForGS];
                        break;
                }

                foreach (int endNodeIndex in endingNodeIdexes)
                {
                    int countMatch = (endNodeIndex - startNodeIndex) + 1;
                    if (_segments[endNodeIndex].Contains(matchingValue)&&_segments[endNodeIndex].Contains(countMatch.ToString()))
                    {
                        blockIndexes.Add(startNodeIndex, endNodeIndex);
                        break;
                    }
                }
            }

            foreach (DictionaryEntry item in blockIndexes)
            {
                int start = int.Parse(item.Key.ToString());
                int end = int.Parse(item.Value.ToString());
                string[] block = new string[(end - start) + 1];
                Array.Copy(_segments, start, block, 0, (end - start) + 1);
                envelopeBlocks.Add(block);
            }

            return envelopeBlocks;
        }



        private EDIFileType ReturnEDIFileType()
        {
            EDIFileType fileType = EDIFileType.Unknown;
            var stQuery = from item in _segments
                          where item.StartsWith("ST")
                          select item;

            string[] stLine = stQuery.FirstOrDefault().Split(_dataSeparator);
            string fileCode = stLine[1];
            switch (fileCode)
            {
                case "846":
                    fileType = EDIFileType.RURD;
                    break;
                case "873":
                    var bgnQuery = from item in _segments
                                   where item.StartsWith("BGN")
                                   select item;
                    string[] bgnLine = bgnQuery.FirstOrDefault().Split(_dataSeparator);
                    string capFileCode = bgnLine[7];
                    switch (capFileCode)
                    {
                        case "US":
                            fileType = EDIFileType.UNSC;
                            break;
                        case "OA":
                            fileType = EDIFileType.OACY;
                            break;
                        case "Q1":
                            fileType = EDIFileType.SQTS;
                            break;
                        case "Q2":
                            fileType = EDIFileType.SQTSOP;
                            break;
                    }
                    break;
                case "864":
                    fileType = EDIFileType.SWNT;
                    break;
                case "997":
                    fileType = EDIFileType.Ack;
                    break;
                case "874":
                    fileType = EDIFileType.NMQR;
                    break;
            }
            return fileType;
        }
    }
}
