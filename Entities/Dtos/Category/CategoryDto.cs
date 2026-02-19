namespace Entities.Dtos.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        /// Kategorinin ilk fotoğrafı (SequenceNumber'a göre).
        /// </summary>
        public string ImagePath { get; set; }
    }
}
