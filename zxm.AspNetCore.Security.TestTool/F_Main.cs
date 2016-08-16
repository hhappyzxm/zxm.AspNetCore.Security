using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            var result = await
                HmacClient11.PostAsync(txtUri.Text, txtClientId.Text, txtClientSecret.Text, null, txtRequestBody.Text);

            txtResponse.Text = JsonConvert.SerializeObject(result);
        }
    }
}
