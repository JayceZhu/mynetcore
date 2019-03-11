namespace Model.CommandData
{
    public class LoginOAuthData
    {
        public LoginOAuthData()
        {
            ErrorCode = 0;
        }
        public string NickName { get; set; }

        public string Sex { get; set; }

        public string UnionId { get; set; }

        public string OpenId { get; set; }

         public string PhotoUrl { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

    }
}
