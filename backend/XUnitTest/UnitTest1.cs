using Xunit;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spec_Project.Controllers;
using Spec_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Spec_Project.Models;

namespace TestProject
{
    //[TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        [Fact]
        public void TestGetIDCompany()
        {
            var mock = new Mock<ICompanyService>();
            mock.Setup(p => p.getIDCompany("6")).Returns("Jignesh");
            CompanyController home = new CompanyController(mock.Object);
            var result = (OkObjectResult)home.getIDCompany("6");
            var x = (OkObjectResult)home.getIDCompany("6");
            Assert.Equal(result.Value, x.Value);
        }

        //[Fact]
        //public void TestCompany()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<ICompanyService>();
        //    mockRepo.Setup(repo => repo.getCompany())
        //        .Returns(GetAllItems());
        //    var controller = new CompanyController(mockRepo.Object);

        //    // Act
        //    var result = controller.GetAllItems();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<List<TblCustomer>>(
        //        viewResult.ViewData.Model);
        //    Assert.Equal(2, model.Count());
        //}


        [Fact]
        public void TestAddCompany()
        {
            var newadd = GetTest();
            // Arrange
            var mockRepo = new Mock<ICompanyService>();
            mockRepo.Setup(p => p.addCompany(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "ds"

            });
            var controller = new CompanyController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.addCompany(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("ds", actualResponse.Data);
            mockRepo.Verify();
        }

        private CustomerModel GetTest()
        {
            var x = new CustomerModel()
            {
                Cid = "s1",
                Name = "John",
                Address = "dsad",
            };
            return x;
        }

        [Fact]
        public void TestDeleteCompany()
        {
            var mock = new Mock<ICompanyService>();
            mock.Setup(p => p.DeleteCompany("6")).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            CompanyController home = new CompanyController(mock.Object);
            var result = (OkObjectResult)home.DeleteCompany("6");
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestEditCompany()
        {
            var newadd = GetTest();
            // Arrange
            var mockRepo = new Mock<ICompanyService>();
            mockRepo.Setup(p => p.EditCompany(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new CompanyController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.EditCompany(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }

        // Test ScanData

        private ScanDataModel GetAddScandata()
        {
            var x = new ScanDataModel()
            {
                Uid = 3,
                ScanId = "John",
                CreatedOn = "dsad",
                Payload = "vcn",
                DataType = "string",
                FileName = "ads",
                Status = 1
            };
            return x;
        }

        //[Fact]
        //public void TestgetScanData()
        //{
        //    var mock = new Mock<IScanDataService>();
        //    mock.Setup(p => p.getScanData()).Returns(getScanData());
        //    CompanyController home = new CompanyController(mock.Object);
        //    var result = (OkObjectResult)home.getScanData("6");
        //    var x = (OkObjectResult)home.getScanData("6");
        //    result.Should().BeEquivalentTo(x);
        //}



        [Fact]
        public void TestaddScanData()
        {
            var newadd = GetAddScandata();
            // Arrange
            var mockRepo = new Mock<IScanDataService>();
            mockRepo.Setup(p => p.addScanData(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new ScanDataController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.addScanData(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }



        [Fact]
        public void TestDeleteScanData()
        {
            var mock = new Mock<IScanDataService>();
            mock.Setup(p => p.DeleteScanData("6")).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"
            });
            ScanDataController home = new ScanDataController(mock.Object);
            var result = (OkObjectResult)home.DeleteScanData("6");
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
        }

        [Fact]
        public void TestEditScandata()
        {
            var newadd = GetAddScandata();
            // Arrange
            var mockRepo = new Mock<IScanDataService>();
            mockRepo.Setup(p => p.EditScandata(newadd)).Returns(new Spec_Project.Models.ResponseModel
            {
                Data = "done"

            });
            var controller = new ScanDataController(mockRepo.Object);

            // Act
            var result = (OkObjectResult)controller.EditScandata(newadd);
            //result.Value
            // Assert
            var actualResponse = Assert.IsType<ResponseModel>(result.Value);
            Assert.Equal("done", actualResponse.Data);
            mockRepo.Verify();
        }
    }
}