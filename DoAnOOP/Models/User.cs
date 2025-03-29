namespace DoAnOOP.Models
{
    // 6. Lớp User đại diện cho người dùng làm bài test
    public class User
    {
        private string name;

        public User(string nameInput)
        {
            name = nameInput;
        }

        public string Name
        {
            get { return name; }
        }
    }
}