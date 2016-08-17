namespace zxm.AspNetCore.Security.TestTool
{
    partial class F_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRequest = new System.Windows.Forms.Button();
            this.txtUri = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClientSecret = new System.Windows.Forms.TextBox();
            this.txtRequestBody = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUserToken = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRequest
            // 
            this.btnRequest.Location = new System.Drawing.Point(808, 65);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(75, 23);
            this.btnRequest.TabIndex = 0;
            this.btnRequest.Text = "Request";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // txtUri
            // 
            this.txtUri.Location = new System.Drawing.Point(50, 30);
            this.txtUri.Name = "txtUri";
            this.txtUri.Size = new System.Drawing.Size(833, 21);
            this.txtUri.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Uri";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Client Id";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(86, 67);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(162, 21);
            this.txtClientId.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Client Secret";
            // 
            // txtClientSecret
            // 
            this.txtClientSecret.Location = new System.Drawing.Point(343, 67);
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.Size = new System.Drawing.Size(169, 21);
            this.txtClientSecret.TabIndex = 5;
            // 
            // txtRequestBody
            // 
            this.txtRequestBody.Location = new System.Drawing.Point(23, 122);
            this.txtRequestBody.Multiline = true;
            this.txtRequestBody.Name = "txtRequestBody";
            this.txtRequestBody.Size = new System.Drawing.Size(860, 214);
            this.txtRequestBody.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Request Body";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 359);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Response";
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(20, 374);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(863, 244);
            this.txtResponse.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(518, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 11;
            this.label9.Text = "User Token";
            // 
            // txtUserToken
            // 
            this.txtUserToken.Location = new System.Drawing.Point(589, 67);
            this.txtUserToken.Name = "txtUserToken";
            this.txtUserToken.Size = new System.Drawing.Size(186, 21);
            this.txtUserToken.TabIndex = 12;
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 641);
            this.Controls.Add(this.txtUserToken);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRequestBody);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtClientSecret);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUri);
            this.Controls.Add(this.btnRequest);
            this.Name = "F_Main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClientSecret;
        private System.Windows.Forms.TextBox txtRequestBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUserToken;
    }
}

