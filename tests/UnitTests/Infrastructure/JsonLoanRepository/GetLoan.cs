using NSubstitute;
using Library.ApplicationCore;
using Library.ApplicationCore.Entities;
using Library.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTests.Infrastructure.JsonLoanRepositoryTests;

public class GetLoanTest
{
    private readonly ILoanRepository _mockLoanRepository;
    private readonly JsonLoanRepository _jsonLoanRepository;
    private readonly IConfiguration _configuration;
    private readonly JsonData _jsonData;

    public GetLoanTest()
    {
        _mockLoanRepository = Substitute.For<ILoanRepository>();
        _configuration = new ConfigurationBuilder().Build();
        _jsonData = new JsonData(_configuration);
        _jsonLoanRepository = new JsonLoanRepository(_jsonData);
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns loan when loan ID is found")]
    public async Task GetLoan_ReturnsLoan_WhenLoanIdFound()
    {
        // Arrange
        var loanId = 1;
        var expectedLoan = new Loan
        {
            Id = loanId,
            BookItemId = 17,
            PatronId = 22,
            LoanDate = new DateTime(2023, 12, 8, 0, 40, 43, 180),
            DueDate = new DateTime(2023, 12, 22, 0, 40, 43, 180),
            ReturnDate = null
        };
        _mockLoanRepository.GetLoan(loanId).Returns(expectedLoan);

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(loanId);

        // Assert
        Assert.NotNull(actualLoan);
        Assert.Equal(expectedLoan.Id, actualLoan.Id);
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns null when loan ID is not found")]
    public async Task GetLoan_ReturnsNull_WhenLoanIdNotFound()
    {
        // Arrange
        var loanId = 999; // Assuming this ID does not exist
        _mockLoanRepository.GetLoan(loanId).Returns((Loan?)null);

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(loanId);

        // Assert
        Assert.Null(actualLoan);
    }

    
}