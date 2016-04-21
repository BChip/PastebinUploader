using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text;
            text = Clipboard.GetText();
            if (text.Length != 0 && textBox1.Text.Length != 0)
            {
            Upload:

                System.Collections.Specialized.NameValueCollection Data
                    = new System.Collections.Specialized.NameValueCollection();

                Data["api_paste_name"] = textBox1.Text; //Title
                Data["api_paste_expire_date"] = "2W"; //2 Weeks
                Data["api_paste_code"] = text;
                Data["api_dev_key"] = "#";
                Data["api_option"] = "paste";
                Data["$api_paste_private"] = "1"; // 0=public 1=unlisted 2=private

                WebClient wb = new WebClient();
                byte[] bytes = wb.UploadValues("http://pastebin.com/api/api_post.php", Data);

                string response;
                using (MemoryStream ms = new MemoryStream(bytes))
                using (StreamReader reader = new StreamReader(ms))
                    response = reader.ReadToEnd();

                if (response.StartsWith("Bad API request"))
                {
                    if (MessageBox.Show("Failed to upload!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                        goto Upload;
                }
                else
                {
                    response = "http://pastebin.com/raw.php?i=" + response.Substring(20);

                    Clipboard.SetText(response);
                    textBox1.Text = "YOUR PASTE: " + response;
                    textBox1.Enabled = true;
                    MessageBox.Show("Succesfully uploaded! \r\nLink copied to clipboard.", "Success!");
                    System.Diagnostics.Process.Start(response);
                }
                
            }
            else
            {
                MessageBox.Show("NO DATA IN CLIPBOARD!");
            }
            
        }
    }
}
