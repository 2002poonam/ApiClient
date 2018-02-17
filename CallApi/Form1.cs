using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;





namespace CallApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            lblStatus.Visible = false;


        }

        private void btnGet_Click(object sender, EventArgs e)
        {
           //Call the webapi 
            Get();

        }

       
        /// <summary>
        /// Get all storeItems from webApi
        /// </summary>
        private async void Get()
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://www.nutidweboffice.com/bunkersgolfhk/api/assignment"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var itemJsonString = await response.Content.ReadAsStringAsync();
                        List<StoreItem> lstStoreItem;
                       
                        lstStoreItem = JsonConvert.DeserializeObject<StoreItem[]>(itemJsonString).ToList();

                        //Populate the gridview
                        Populate(lstStoreItem);

                    }
                }
            }

        }


        /// <summary>
        /// Method to populate gridview with storeitem list
        /// </summary>
        /// <param name="lstStoreItem"></param>
        private void Populate(List<StoreItem> lstStoreItem)
        {
            
            grdArticles.DataSource = null;
            grdArticles.Rows.Clear();
            grdArticles.Refresh();
            grdArticles.Columns.Clear();

            int i = 0;
           
            grdArticles.Columns.Add("ID", "ID");
            grdArticles.Columns.Add("ItemId", "ItemId");
            grdArticles.Columns.Add("StoreItemDescriptions", "StoreItemDescriptions");
            grdArticles.Columns.Add("Sellprice", "Sellprice");
            grdArticles.Columns.Add("StoreDepartment", "StoreDepartment Name");
            grdArticles.Columns.Add("Barcode", "Barcode");
            grdArticles.Columns.Add("StoreItemStorageAmounts", "StorageSpot Description");
            grdArticles.Columns.Add("ItemNumber", "ItemNumber");
            grdArticles.Columns.Add("CatalogNumber", "CatalogNumber");
            grdArticles.Columns.Add("Extra1", "Extra1");
            grdArticles.Columns.Add("Deleted", "Deleted");

            foreach (var storeItem in lstStoreItem)
            {
                
                i++;

                DataGridViewRow row = new DataGridViewRow();

                row.CreateCells(grdArticles);
                row.Cells[0].Value = storeItem.Id;
                row.Cells[1].Value = storeItem.ItemId;
                row.Cells[2].Value = storeItem.StoreItemDescriptions[0].Description;
                row.Cells[3].Value = storeItem.StoreItemPrices[0].Sellprice; 
                row.Cells[4].Value = storeItem.StoreDepartment.StoreDepartmentNames[0].Name;
                row.Cells[5].Value = storeItem.Barcode;
                row.Cells[6].Value = storeItem.StoreItemStorageAmounts[0].StorageSpot.Description;
                row.Cells[7].Value = storeItem.ItemNumber;
                row.Cells[8].Value = storeItem.CatalogNumber;
                row.Cells[9].Value = storeItem.Extra1;
                row.Cells[10].Value = storeItem.Deleted;
               

                grdArticles.Rows.Add(row);
                
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            //Check storeitems count is 3 or not
            ItemsInStock();
        }

        
        /// <summary>
        /// Method for Checking storeitems count is 3 or not.And based on that show the status.
        /// </summary>
        private async void ItemsInStock()
        {

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://www.nutidweboffice.com/bunkersgolfhk/api/assignment"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var itemJsonString = await response.Content.ReadAsStringAsync();
                        int cntStoreItem;


                        cntStoreItem = JsonConvert.DeserializeObject<StoreItem[]>(itemJsonString).ToList().Count;
                         
                       if (cntStoreItem!=3)
                        {
                            lblStatus.Visible = true;
                            
                            var msgResponse = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                            lblStatus.Text = "Status:-"+ msgResponse.StatusCode + " -StoreItems count is not equal to 3. ";
                        }
                       else
                        {
                            lblStatus.Visible = true;

                            var msgResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                            lblStatus.Text = "Status:-" + msgResponse.StatusCode  + " -StoreItems count is equal to 3. ";
                        }



                    }
                }
            }
           
        }

       
    }
}
