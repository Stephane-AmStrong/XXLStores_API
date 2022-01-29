namespace Application.Features.AppUsers.Queries.GetPagedList
{
    public record GetAppUsersViewModel
    {
        public string Id { get; set; }
        public string ImgLink { get; set; }
        public string Firstname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
