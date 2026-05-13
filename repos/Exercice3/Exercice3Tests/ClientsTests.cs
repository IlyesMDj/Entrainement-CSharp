using Exercice3.Models;
using Xunit;

namespace Exercice3Tests
{
    public class ClientsTests
    {
        [Fact]
        public void NouveauClientAUneListeVide()
        {
            var client = new Client();
            client.Nom = "Test";

            Assert.NotNull(client.Commandes);
            Assert.Empty(client.Commandes);
        }
    }
}
