<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SPLConqueror_WebSite._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Welcome to SPLConqueror!</h1>
        <p class="lead" style="height: 26px">Current status:</p>
        <p class="lead" style="height: 28px">
            <asp:Label ID="status" runat="server"></asp:Label>
        </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Model editor</h2>
            <p>
                Create your own feature model. Add and edit your features and connect them with complex constraints.
            </p>
            <p>
                <a class="btn btn-default" href="ModelEditor">Go to model editor &raquo;</a>
            </p>
        </div>

        <div class="col-md-4">
            <h2>Function Learning</h2>
            <p>
                Learn the performance function of a feature model. You can use your created model or you can load in a saved model.
            </p>
            <p>
                <a class="btn btn-default" href="FunctionLearning">Go to learning &raquo;</a>
            </p>
        </div>

        <div class="col-md-4">
            <h2>Function Analyzer</h2>
            <p>
                Analyze and adjust your learned function to display information about it.
            </p>
            <p>
                <a class="btn btn-default" href="FunctionAnalyzer">Go to function analyzer &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
