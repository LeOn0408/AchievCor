using AchievCor.Server.Data.Authorization;

namespace AchievCor.Server.Test
{
    public class PasswordTest
    {
        [Fact]
        public void VerifyPasswordMatches()
        {
            string pass = "tester";

            string hasspass = Password.Hash(pass);

            bool verify = Password.Verify(pass, hasspass);

            Assert.True(verify);
        }
    }
}