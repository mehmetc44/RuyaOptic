namespace RuyaOptik.DTO.Category
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }
}
