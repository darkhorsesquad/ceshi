//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FriendFile_Library
    {
        public FriendFile_Library()
        {
            this.FriendFile_Details = new HashSet<FriendFile_Details>();
        }
    
        public int FriendFile_LibraryID { get; set; }
        public int Conversation_ID { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int Contain { get; set; }
    
        public virtual Friend_Conversation Friend_Conversation { get; set; }
        public virtual ICollection<FriendFile_Details> FriendFile_Details { get; set; }
    }
}