using Enoch.CrossCutting;
using Xunit;

namespace CrossCutting.Tests
{
    public class ExtensionsTest
    {
        [Theory(DisplayName = "CNPJ Validation, must return Success")]
        [Trait("CrossCutting", "CNPJ Validation")]
        [InlineData(57510754000109)]
        [InlineData(74677109000102)]
        [InlineData(01848163000191)]
        [InlineData(40003318000140)]
        [InlineData(00159552000147)]

        public void Extensions_ValidateCNPJ_MustReturnTrue(long cnpj)
        {
            //Arrange & Act
            var validCnpj = cnpj.IsCNPJ();

            //Assert
            Assert.True(validCnpj);
        }

        [Theory(DisplayName = "CNPJ Validation, must return Error")]
        [Trait("CrossCutting", "CNPJ Validation")]
        [InlineData(111222333444555)]
        [InlineData(7467000102)]
        [InlineData(01845555558163000191)]
        [InlineData(400)]
        [InlineData(1111111111111111)]
        public void Extensions_ValidateCNPJ_MustReturnError(long cnpj)
        {
            //Arrange & Act
            var invalidCnpj = cnpj.IsCNPJ();

            //Assert
            Assert.False(invalidCnpj);
        }

        [Fact(DisplayName = "Encryption, must return Success")]
        [Trait("CrossCutting", "String encryption")]
        public void Extensions_Encrypt_MustReturnSuccess()
        {
            //Arrange
            var plainText = "testecriptografia";
            var encryptedText = "dcSGqcQ+0RTRGpqF85SxX5QTlyyUD4PxeEdG0HFZRtZCwCjMrHrrvlOoihPW0EgJ";

            //Act
            var encryptResult = Encryption.Encrypt(plainText);

            //Assert
            Assert.Equal(encryptedText, encryptResult);
            Assert.NotEmpty(encryptResult);
        }

        [Fact(DisplayName = "Encryption, must return Error")]
        [Trait("CrossCutting", "String encryption")]
        public void Extensions_Encrypt_MustReturnError()
        {
            //Arrange
            var plainText = "testecriptografia";
            var encryptedText = "dcSGqcQ+0RTRGpqF85SxX5QTlyyUD4PxeEdG0HFZRtZCwCjMrHrrvlOoihPW0EgJs";

            //Act
            var encryptResult = Encryption.Encrypt(plainText);

            //Assert
            Assert.NotEqual(encryptedText, encryptResult);
        }

        [Fact(DisplayName = "Decrypt, must return Success")]
        [Trait("CrossCutting", "String decryption")]
        public void Extensions_Decrypt_MustReturnSuccess()
        {
            //Arrange
            var plainText = "descriptografar";
            var encryptedText = "GAL/qL874Q6NcC8FxBD2tIciVa6HDmZ2QDoGZs4gXnc=";


            //Act
            var decryptedResult = Encryption.Decrypt(encryptedText);

            //Assert
            Assert.Equal(plainText, decryptedResult);
            Assert.NotEmpty(decryptedResult);
        }

        [Fact(DisplayName = "Decrypt, must return Error")]
        [Trait("CrossCutting", "String decryption")]
        public void Extensions_Decrypt_MustReturnError()
        {
            //Arrange
            var plainText = "descriptografdar";
            var encryptedText = "GAL/qL874Q6NcC8FxBD2tIciVa6HDmZ2QDoGZs4gXnc=";

            //Act
            var decryptedResult = Encryption.Decrypt(encryptedText);

            //Assert
            Assert.NotEqual(plainText, decryptedResult);
        }
    }
}
