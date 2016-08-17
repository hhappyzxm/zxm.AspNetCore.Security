using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using zxm.AspNetCore.Hmac.Client;

namespace zxm.AspNetCore.Security.TestTool
{
    public partial class F_Main : Form
    {
        public F_Main()
        {
            InitializeComponent();
        }

        private async void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await
                    HmacClient.PostAsync(txtUri.Text, txtClientId.Text, txtClientSecret.Text, txtUserToken.Text,
                        txtRequestBody.Text);

                if (response.ResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    txtResponse.Text = $"Request failed, state code is {response.ResponseMessage.StatusCode}";
                }
                else
                {
                    txtResponse.Text = await response.ResponseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                txtResponse.Text = ex.Message;
            }
        }
    }
}
