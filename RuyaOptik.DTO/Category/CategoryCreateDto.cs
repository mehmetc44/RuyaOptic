namespace RuyaOptik.DTO.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }

}
