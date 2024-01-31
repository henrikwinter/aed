//using Spire.Doc;
//using Spire.Doc.Documents;
using HiQPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Dextra.Report
{
    public class Html2Pdf
    {
        public byte[] Bdata { get; set; }
        public Html2Pdf(string htmlToConvert,string baseUrl)
        {
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // set browser width
            //htmlToPdfConverter.BrowserWidth = 800;// int.Parse(textBoxBrowserWidth.Text);
            //htmlToPdfConverter.BrowserHeight = 1200;

            htmlToPdfConverter.Document.PageSize = GetSelectedPageSize("A4");
            htmlToPdfConverter.Document.PageOrientation = GetSelectedPageOrientation("Portrait");
            htmlToPdfConverter.Document.Margins = new PdfMargins(10,10,10,10);

            try
            {
                Regex r1 = new Regex(@"<meta property=""dx:pdfdata"" content=""(.*?)"">");
                htmlToConvert = Regex.Replace(htmlToConvert, @"\r\n?|\n|\t", "");
                Match m1 = r1.Match(htmlToConvert);
                if (m1.Success)
                {
                    string work = m1.Groups[1].Value;
                    string[] param = work.Split('.');
                    string[] param1 = null;

                    foreach (string s in param)
                    {
                        param1 = s.Split('=');
                        switch (param1[0])
                        {
                            case "PageSize":
                                htmlToPdfConverter.Document.PageSize = GetSelectedPageSize(param1[1]);
                                break;
                            case "PageOrientation":
                                htmlToPdfConverter.Document.PageOrientation = GetSelectedPageOrientation(param1[1]);
                                break;
                            case "FitPageWidth":
                                htmlToPdfConverter.Document.FitPageWidth = bool.Parse(param1[1]);
                                break;
                            case "FitPageHeight":
                                htmlToPdfConverter.Document.FitPageHeight = bool.Parse(param1[1]);
                                break;
                            case "ForceFitPageWidth":
                                htmlToPdfConverter.Document.ForceFitPageWidth = bool.Parse(param1[1]);
                                break;
                            case "BrowserWidth":
                                htmlToPdfConverter.BrowserWidth = int.Parse(param1[1]);
                                break;
                            case "BrowserHeight":
                                htmlToPdfConverter.BrowserHeight = int.Parse(param1[1]);
                                break;
                            default:
                                break;
                        }
                    }

                }

            }
            catch
            {

            }

            // render the HTML code as PDF in memory
          Bdata = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

        }

        private PdfPageSize GetSelectedPageSize(string pdfPageSizeString)
        {
            switch (pdfPageSizeString)
            {
                case "A0":
                    return PdfPageSize.A0;
                case "A1":
                    return PdfPageSize.A1;
                case "A10":
                    return PdfPageSize.A10;
                case "A2":
                    return PdfPageSize.A2;
                case "A3":
                    return PdfPageSize.A3;
                case "A4":
                    return PdfPageSize.A4;
                case "A5":
                    return PdfPageSize.A5;
                case "A6":
                    return PdfPageSize.A6;
                case "A7":
                    return PdfPageSize.A7;
                case "A8":
                    return PdfPageSize.A8;
                case "A9":
                    return PdfPageSize.A9;
                case "ArchA":
                    return PdfPageSize.ArchA;
                case "ArchB":
                    return PdfPageSize.ArchB;
                case "ArchC":
                    return PdfPageSize.ArchC;
                case "ArchD":
                    return PdfPageSize.ArchD;
                case "ArchE":
                    return PdfPageSize.ArchE;
                case "B0":
                    return PdfPageSize.B0;
                case "B1":
                    return PdfPageSize.B1;
                case "B2":
                    return PdfPageSize.B2;
                case "B3":
                    return PdfPageSize.B3;
                case "B4":
                    return PdfPageSize.B4;
                case "B5":
                    return PdfPageSize.B5;
                case "Flsa":
                    return PdfPageSize.Flsa;
                case "HalfLetter":
                    return PdfPageSize.HalfLetter;
                case "Ledger":
                    return PdfPageSize.Ledger;
                case "Legal":
                    return PdfPageSize.Legal;
                case "Letter":
                    return PdfPageSize.Letter;
                case "Letter11x17":
                    return PdfPageSize.Letter11x17;
                case "Note":
                    return PdfPageSize.Note;
                default:
                    return PdfPageSize.A4;
            }
        }

        private PdfPageOrientation GetSelectedPageOrientation(string pdfPageOrientationString)
        {
            return (pdfPageOrientationString == "Portrait") ? PdfPageOrientation.Portrait : PdfPageOrientation.Landscape;
        }


    }
}