using AutoMapper;
using Exercice3;
using Exercice3.Models;
using Exercice3.Services;
using Moq;
using Xunit;

namespace Exercice3Tests
{
    public class ProduitsViewModelTests
    {
        [Fact]
        public async Task AjouterProduit_DoitViderLesChamps_EtAppelerLeService()
        {
            var mockService = new Mock<IProduitService>();

            var mockMapper = new Mock<IMapper>();

            var viewModel = new ProduitsViewModel(mockService.Object, mockMapper.Object);

            viewModel.NouveauNom = "PC Portable";
            viewModel.NouveauPrix = 1000;
            viewModel.NouvelleQuantite = 5;

            await viewModel.AjouterProduitCommand.ExecuteAsync(null);

            mockService.Verify(s => s.AjouterProduitAsync(It.Is<Produit>(p =>
                p.Nom == "PC Portable" &&
                p.Prix == 1000 &&
                p.Quantite == 5
            )), Times.Once);

            Assert.Equal(string.Empty, viewModel.NouveauNom);
            Assert.Equal(0, viewModel.NouveauPrix);
            Assert.Equal(0, viewModel.NouvelleQuantite);
        }
    }
}
