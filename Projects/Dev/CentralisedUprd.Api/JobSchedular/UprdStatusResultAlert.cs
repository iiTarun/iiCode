using CentralisedUprd.Api.Repositories;
using CentralisedUprd.Api.Services;
using CentralisedUprd.Api.UPRD.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace CentralisedUprd.Api.JobSchedular
{
    public class UprdStatusResultAlert
    {
        public void GenerateUprdStatusFireAlert(DateTime date)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~/App_Data/TestFile.txt"));
            List<string> lines = new List<string>();
            try
            {
                UprdStatusRepository uprdStatusRepo = new UprdStatusRepository();
                List<UPRDStatusDTO> uprdList = new List<UPRDStatusDTO>();
                string Environment = ConfigurationManager.AppSettings.Get("Environment");
                lines.Add("Uprd Job Called");
                uprdList = uprdStatusRepo.GetUprdOnDateAndPipelineDuns(date);
                lines.Add("Uprd List count -- " + uprdList.Count);
                if (uprdList != null)
                {
                    lines.Add("Inside if loop");
                    string EmailContent = BuildEmailTemplate(uprdList.OrderBy(a => a.Pipeline).ToList());
                    string subject = string.Format("Uprd Status Alerts dated: {0} from {1}", DateTime.Now.Date.ToString("MM/dd/yyyy"), Environment);
                    string FilePath = Path.Combine(HostingEnvironment.MapPath("~/App_Data/UprdEmails.csv"));
                    var recipients = File.ReadAllLines(FilePath).Select(a => a).ToArray();
                    string from = ConfigurationManager.AppSettings.Get("EmailIdForAlert");
                    #region Send Email
                    var isSend = EmailandSMSservice.SendGmail(subject, EmailContent, recipients, from);
                    #endregion
                    System.IO.File.WriteAllLines(path, lines.ToArray());
                }
                lines.Add("Out of loop now");
                System.IO.File.WriteAllLines(path, lines.ToArray());
            }
            catch (Exception ex)
            {
                lines.Add(ex.Message + " --> " + ex.ToString());
                System.IO.File.WriteAllLines(path, lines.ToArray());
            }
        }
        #region Building Template for Email
        private string BuildEmailTemplate(List<UPRDStatusDTO> customUPRDReq)
        {

            string emailbody = "<div style=\" display: inline-block; width:100%;heingh:600px\"><div style=\" display: inline-block;width:10%;height:20px;\"></div><div class=\"content\" style=\" display: inline-block;width:75%;height:600px; background:#fff;\"><div style=\"width:100%;height:80px; background:#ffff;\"><img src=\"http://test.natgashub.com/Assets/OrangeNom1DoneLogo.jpg\" alt =\"NatGasHub\" height=\"60px\" width=\"100px\"/><b>";
            string tableStr = "";
            if (customUPRDReq != null)
            {
                emailbody += "</b></div><div style=\"width:100%;height: 500px;overflow: auto; background:#ffff;\">";
                tableStr += "<table width=\"100%\" bgcolor=\"#f6f8f1\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">";
                tableStr += "<thead><tr style=\"background-color:#FF6C3A; color:#fff; height:30px\"><th style=\"text-align:center;\">Pipeline</th><th style=\"text-align:center;\">Post Start Date</th><th style=\"text-align:center;\">Dataset</th><th style=\"text-align:center;\">RURD Received</th><th style=\"text-align:center;\">Dataset Available</th><th style=\"text-align:center;\">Dataset Recieved</th></tr></thead>";
                tableStr += "<tbody>";
                foreach (var item in customUPRDReq)
                {
                    tableStr += "<tr>";
                    tableStr += "<td style=\"text-align:start;\">" + item.Pipeline + "</td>";
                    tableStr += "<td style=\"text-align:start;\">" + item.CreatedDate.Value.ToString("MM/dd/yyyy") + "</td>";
                    tableStr += "<td style=\"text-align:start;\">" + item.DatasetSummary + "</td>";
                    if (item.IsRURDReceived)
                        tableStr += "<td style=\"text-align:center; color:green;\">" + "Y" + "</td>";
                    else
                        tableStr += "<td style=\"text-align:center; color:red;\">" + "N" + "</td>";
                    if (item.IsDataSetAvailable)
                        tableStr += "<td style=\"text-align:center; color:green;\">" + "Y" + "</td>";
                    else
                        tableStr += "<td style=\"text-align:center; color:red;\">" + "N" + "</td>";
                    if (item.IsDatasetReceived)
                        tableStr += "<td style=\"text-align:center; color:green;\">" + "Y" + "</td>";
                    //else if (item.IsDataSetAvailable == true && item.IsDatasetReceived == false)
                    //    tableStr += "<td style=\"text-align:center; color:#fff; background-color:red\">" + "N" + "</td></tr>";
                    else
                        tableStr += "<td style=\"text-align:center;  color:#fff; background-color:red\">" + "N" + "</td>";
                    tableStr += "</tr>";
                }
                tableStr += "</tbody></table>";
                emailbody = emailbody + tableStr;
                emailbody += "</div>";
                emailbody += "<br/>";
                emailbody = emailbody + "<div style=\"width:100%;height:50px; background:#ffff;\">Copyright © NatGasHub.com. <span style=\"color:#FF6C3A\">Forwarding of this data is a copyright violation under U.S. law.</span></div></div><div style=\" display: inline-block;width:10%;height:20px;\"></div></div>";

                return emailbody;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}