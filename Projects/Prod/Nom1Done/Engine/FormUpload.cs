using Nom1Done.Common;
using Nom1Done.Data.Repositories;
using Nom1Done.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Nom1Done.EDIEngineSendAndReceive
{
    public class FormUpload
    {
        private readonly Encoding encoding = Encoding.UTF8;
        IOutbox_MultipartFormRepository _serviceOutboxMultipartForm;
        public FormUpload(IOutbox_MultipartFormRepository _serviceOutboxMultipartForm)
        {
            this._serviceOutboxMultipartForm = _serviceOutboxMultipartForm;
        }
        public HttpWebResponse MultipartFormDataPost(Int32 OutboxID, string postUrl, string userAgent, Dictionary<string, object> postParameters, string username, string password )
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary, OutboxID);

            return PostForm(OutboxID, postUrl, userAgent, contentType, formData, username, password);
        }

        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary, int OutboxID)
        {
            
            //IOutbox_MultipartFormRepository _serviceOutboxMultipartForm = Kernal.Get<Outbox_MultipartFormRepository>();
            //IUnitOfWork unitOfWork = Kernal.Get<UnitOfWork>();
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            string FormatDate = Format.GisbDate();
            try
            {
                foreach (var param in postParameters)
                {
                    if (needsCLRF)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                    needsCLRF = true;
                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;
                        string header = string.Format(
                            "--{0}\r\nContent-disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\ncontent-type: {3}; boundary=\"--boundary2--" + FormatDate +
                            "\"; protocol=\"application/pgp-encrypted\"\r\n\r\n----boundary2--" + FormatDate +
                            "\r\ncontent-type: application/pgp-encrypted\r\n\r\nVersion: 1\r\n\r\n----boundary2--"
                            + FormatDate + "\r\ncontent-type: application/octet-stream\r\n\r\n",
                            boundary,
                            param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.ContentType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }
                string footer = "----boundary2--" + FormatDate + "--\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();

                Outbox_MultipartForm multipartForm = new Outbox_MultipartForm();
                multipartForm.OutboxID = OutboxID;
                multipartForm.Data = System.Text.Encoding.UTF8.GetString(formData).ToString();
                _serviceOutboxMultipartForm.Add(multipartForm);//BLLOutboxMultipartForm.Save(multipartForm);
                _serviceOutboxMultipartForm.Save();
                return formData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private HttpWebResponse PostForm(Int32 OutboxID, string postUrl, string userAgent, string contentType, byte[] formData, string username, string password)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            try
            {
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.CookieContainer = new CookieContainer();
                request.ContentLength = formData.Length;
                request.PreAuthenticate = true;
                request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(username + ":" + password)));
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }
                return request.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
}
