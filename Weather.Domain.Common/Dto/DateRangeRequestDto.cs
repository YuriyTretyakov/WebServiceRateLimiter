namespace WebApiExploration.Dto
{
    public class DateRangeRequestDto
    {
        public DateTimeOffset After { get; set; }
        public DateTimeOffset Before { get; set; }
    }
}
