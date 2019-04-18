using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Store_dataDto
    {
        public int Store_data_ID { get; set; }
        public int User_ID { get; set; }
        public string DataRoute { get; set; }
        public string FolderName { get; set; }
        public string Real_FileName { get; set; }
        public string SuffixName { get; set; }
        public string File_Size { get; set; }
        public int Folder_ID { get; set; }
        public System.DateTime EstablishTime { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public int DeleteState { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }

        public virtual User User { get; set; }
    }
}
