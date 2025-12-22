namespace RuyaOptik.DTO.System
{
    public class ApiInfoDto
    {
        public string Application { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Environment { get; set; } = null!;
        public DateTime ServerTime { get; set; }
    }
}
