﻿namespace DocSaaSApp.Models
{
    public class UserFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
