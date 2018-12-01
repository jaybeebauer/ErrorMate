using Google.Apis.Customsearch.v1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErrorMate
{
    public partial class MainForm : Form
    {
        const string cssLoader = "<style>.center-div{poaition: absolute; margin: auto; top: 0; right: 0; bottom: 0; left: 0; width: 200px; height: 200px;}.loader{border: 16px solid #f3f3f3; /* Light grey */ border-top: 16px solid #3498db; /* Blue */ border-radius: 50%; width: 120px; height: 120px; animation: spin 2s linear infinite;}@keyframes spin{0%{transform: rotate(0deg);}100%{transform: rotate(360deg);}}</style><div class='center-div'><div class='loader'></div><div>Please wait while we check for results.</div></div>";

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btn_imageError_Click(object sender, EventArgs e)
        {
            dlg_uploadError.ShowDialog();
            DisplayHtml(cssLoader);
            byte[] byteArray = GetImageAsByteArray(dlg_uploadError.FileName);
            await Task.Run(() => GetResults(byteArray));
        }

        private void btn_captureError_Click(object sender, EventArgs e)
        {
            this.Hide();
            SnippingTool.AreaSelected += OnAreaSelected;
            SnippingTool.Snip();
            this.Show();
        }

        private async void OnAreaSelected(object sender, EventArgs e)
        {
            DisplayHtml(cssLoader);
            var bmp = SnippingTool.Image;
            byte[] byteArray = bmp.ToByteArray(ImageFormat.Bmp);
            await Task.Run(() => GetResults(byteArray));
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {

        }

        private async void GetResults(byte[] byteArray)
        {
            try
            {
                IList<Result> results = await ErrorAnalysis.RunBMP(byteArray);

                if (results != null)
                {
                    Uri resultLink = new Uri(results[0].Link);

                    MsSupportJsonResult response = MsSupportJsonResult.Retrieve(results[0].Link);
                    string description = response.details.description.ToString();

                    Body resolution = response.details.body.Where(b => b.title != null && b.title.Trim() == "Resolution").Select(b => b).SingleOrDefault();
                    Body workaround = response.details.body.Where(b => b.title != null && b.title.Trim() == "Workaround").Select(b => b).SingleOrDefault();
                    if (resolution != null)
                    {
                        DisplayHtml("<!DOCTYPE html><html><head><base href = '" + resultLink.GetLeftPart(UriPartial.Authority) + "' /></head><body>" + "<h1>" + resolution.title.ToString() + "</h1>" + resolution.content[0].ToString() + "</body></html>");
                    }
                    else
                    {
                        if (workaround != null)
                        {
                            DisplayHtml("<h1>" + workaround.title.ToString() + "</h1>" + workaround.content[0].ToString());
                        }
                        else
                        {
                            DisplayHtml("We couldn't find any results for this error or something went wrong, check to make sure you only capture the error and no other background images and try again.");
                        }
                    }
                }
                else
                {
                    DisplayHtml("We couldn't find any results for this error or something went wrong, check to make sure you only capture the error and no other background images and try again.");
                }
            }
            catch (Exception e)
            {
                DisplayHtml("We couldn't connect to the cognative service, make sure you are connected to the internet");
                Console.WriteLine(e.InnerException);
            }
        }

        private void DisplayHtml(string html)
        {
            webBrowser1.Navigate("about:blank");
            InvokeUI(() => {
                if (webBrowser1.Document != null)
                {
                    webBrowser1.Document.Write(string.Empty);
                }
                webBrowser1.DocumentText = html;
            });
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                e.Cancel = true;
                Process.Start(e.Url.ToString());
            }

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }

    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
