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

        [Theory(DisplayName = "CPF Validation, must return Success")]
        [Trait("CrossCutting", "CPF Validation")]
        [InlineData(80884904059)]
        [InlineData(26277828002)]
        [InlineData(42933872005)]
        [InlineData(50428153097)]

        public void Extensions_ValidateCPF_MustReturnTrue(long cpf)
        {
            //Arrange & Act
            var validCpf = cpf.IsCpf();

            //Assert
            Assert.True(validCpf);
        }

        [Theory(DisplayName = "CPF Validation, must return Error")]
        [Trait("CrossCutting", "CPF Validation")]
        [InlineData(04059)]
        [InlineData(00000000000)]
        [InlineData(10101010101)]
        [InlineData(42933872999999005)]
        [InlineData(1)]

        public void Extensions_ValidateCPF_MustReturnFalse(long cpf)
        {
            //Arrange & Act
            var invalidCpf = cpf.IsCpf();

            //Assert
            Assert.False(invalidCpf);
        }

        [Theory(DisplayName = "CastCnpj, must return Success")]
        [Trait("CrossCutting", "CPF Casting")]
        [InlineData(80884904059)]
        [InlineData(08019334041)]
        [InlineData(26277828002)]
        [InlineData(42933872005)]
        [InlineData(50428153097)]

        public void Extensions_CastCnpj_MustReturnCPFValueConverted(long cpf)
        {
            //Arrange & Act
            var validCpf = cpf.CastCnpj();

            //Assert
            Assert.NotEmpty(validCpf);
            Assert.StartsWith("000", validCpf);
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

        [Theory(DisplayName = "Email Validation, must return True")]
        [Trait("CrossCutting", "Email Validation")]
        [InlineData("thiago_test_com@email.com")]
        [InlineData("thiago_test_com_br@email.com.br")]
        [InlineData("thiago_net_test@email.net")]
        [InlineData("thiago_test_io@email.io")]
        [InlineData("thiago_teste_org@email.org.br")]

        public void Extensions_ValidateEmail_MustReturnTrue(string email)
        {
            //Arrange & Act
            var validEmail = email.IsValidMail();

            //Assert
            Assert.True(validEmail);
        }

        [Theory(DisplayName = "Email Validation, must return False")]
        [Trait("CrossCutting", "Email Validation")]
        [InlineData("thiago")]
        [InlineData("thiago_test_com_bremail.com.br")]
        [InlineData("thiago_net_test@")]
        [InlineData("@email.io")]

        public void Extensions_ValidateEmail_MustReturnFalse(string email)
        {
            //Arrange & Act
            var invalidEmail = email.IsValidMail();

            //Assert
            Assert.False(invalidEmail);
        }

        [Theory(DisplayName = "Phone Validation, must return Success")]
        [Trait("CrossCutting", "Phone Validation")]
        [InlineData("(19)99123-5080")]
        [InlineData("(11)3720-7536")]
        [InlineData("(011)3720-7536")]
        [InlineData("16 99111-5077")]
        [InlineData("3720-7536")]
        [InlineData("91231-6165")]

        public void Extensions_ValidatePhone_MustReturnTrue(string phone)
        {
            //Arrange & Act
            var validPhone = phone.IsValidPhone();

            //Assert
            Assert.True(validPhone);
        }

        [Theory(DisplayName = "Phone Validation, must return Success")]
        [Trait("CrossCutting", "Phone Validation")]
        [InlineData("")]
        [InlineData("00000000")]

        public void Extensions_ValidatePhone_MustReturnFalse(string phone)
        {
            //Arrange & Act
            var invalidPhone = phone.IsValidPhone();

            //Assert
            Assert.False(invalidPhone);
        }
    }
}
