namespace RouteNavigator
{
    partial class ListViewScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ColumnHeader columnHeaderRoute;
            System.Windows.Forms.ColumnHeader columnHeaderClass;
            System.Windows.Forms.ColumnHeader columnHeaderMethod;
            System.Windows.Forms.ColumnHeader columnHeaderHttpMethod;
            this.lvRoutes = new System.Windows.Forms.ListView();
            this.tbFilter = new System.Windows.Forms.TextBox();
            columnHeaderRoute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeaderClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeaderMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeaderHttpMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // columnHeaderRoute
            // 
            columnHeaderRoute.Text = "Route";
            // 
            // columnHeaderClass
            // 
            columnHeaderClass.Text = "Class";
            // 
            // columnHeaderMethod
            // 
            columnHeaderMethod.Text = "Method";
            // 
            // columnHeaderHttpMethod
            // 
            columnHeaderHttpMethod.Text = "HTTP";
            // 
            // lvRoutes
            // 
            this.lvRoutes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRoutes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeaderHttpMethod,
            columnHeaderRoute,
            columnHeaderClass,
            columnHeaderMethod});
            this.lvRoutes.FullRowSelect = true;
            this.lvRoutes.GridLines = true;
            this.lvRoutes.HideSelection = false;
            this.lvRoutes.Location = new System.Drawing.Point(3, 29);
            this.lvRoutes.Name = "lvRoutes";
            this.lvRoutes.Size = new System.Drawing.Size(599, 378);
            this.lvRoutes.TabIndex = 3;
            this.lvRoutes.UseCompatibleStateImageBehavior = false;
            this.lvRoutes.View = System.Windows.Forms.View.Details;
            this.lvRoutes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnListViewRoutes_ColumnClick);
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(3, 3);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(599, 20);
            this.tbFilter.TabIndex = 2;
            // 
            // ListViewScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvRoutes);
            this.Controls.Add(this.tbFilter);
            this.Name = "ListViewScreen";
            this.Size = new System.Drawing.Size(605, 410);
            this.Load += new System.EventHandler(this.ListViewScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvRoutes;
        private System.Windows.Forms.TextBox tbFilter;
    }
}
