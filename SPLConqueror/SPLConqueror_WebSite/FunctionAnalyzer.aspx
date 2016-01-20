<%@ Page Title="Function Analyzer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FunctionAnalyzer.aspx.cs" Inherits="SPLConqueror_WebSite.FunctionAnalyzer" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <table class="nav-justified">
        <tr>
            <td style="width: 320px">
                <asp:FileUpload ID="modelFileUpload" runat="server" Width="315px" />
            </td>
            <td style="width: 282px">
                <asp:FileUpload ID="functionFileUpload" runat="server" Width="273px" />
            </td>
            <td>
                <asp:FileUpload ID="measurementsFileUpload" runat="server" Width="315px" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 320px">
                <asp:Button ID="loadModelButton" runat="server" Text="Read Variability Model" Width="163px" OnClick="loadModelButton_Click" />
            </td>
            <td style="width: 282px">
                <asp:Button ID="loadFunctionButton" runat="server" Text="Read Function" Width="109px" />
            </td>
            <td>
                <asp:Button ID="loadMeasurementsButton" runat="server" Text="Read Measurements" Width="155px" OnClick="loadMeasurementsButton_Click" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 320px">
                <asp:Label ID="modelErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="Please load a model."></asp:Label>
            </td>
            <td style="width: 282px">
                <asp:Label ID="functionErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="Please load a a function."></asp:Label>
            </td>
            <td>
                <asp:Label ID="measurementsErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="Please load some measurements."></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <p style="width: 943px">

    </p>
<p style="width: 943px; text-decoration: underline">

    <strong>Function:</strong></p>
<table class="nav-justified">
    <tr>
        <td style="width: 260px"><strong>Input function:</strong></td>
        <td><strong>Function configurations:</strong></td>
        <td style="width: 260px"><strong>Output function:</strong></td>
    </tr>
    <tr>
        <td style="width: 260px">
            <asp:TextBox ID="funcInputTextBox" runat="server" Width="250px" Height="275px"></asp:TextBox>
        </td>
        <td>
            <table class="nav-justified">
                <tr>
                    <td>
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 25px" title=" ">
                                    <asp:CheckBox ID="funcDigitsCheckBox" runat="server" Text=" " />
                                </td>
                                <td style="width: 200px">Fractional digits of constants:</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 25px">
                                    <asp:CheckBox ID="funcRelativeConstantCheckBox" runat="server" Text=" " />
                                </td>
                                <td style="width: 200px">Relative value of constants:</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 25px">
                                    <asp:CheckBox ID="funcFilterVarCheckBox" runat="server" Text=" " />
                                </td>
                                <td style="width: 200px">Filter variables:</td>
                                <td class="text-right">
                                    <asp:DropDownList ID="funcFilterOptionDDList" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" Height="156px">
                            <asp:CheckBoxList ID="funcVariableCheckBoxList" runat="server">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 25px">
                                    <asp:CheckBox ID="funcFilterRegexCheckBox" runat="server" Text=" " />
                                </td>
                                <td>Search for variables containing</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="funcFilterRegexTextBox" runat="server" Width="405px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 260px">
            <asp:TextBox ID="funcOutputTextBox" runat="server" Width="250px" Height="275px"></asp:TextBox>
        </td>
    </tr>
</table>
<p style="width: 943px">

    </p>
<p style="width: 943px; text-decoration: underline">

    <strong>Evaluation:p>
<table class="nav-justified">
    <tr>
        <td>
            <table class="nav-justified" style="height: 493px">
                <tr>
                    <td style="height: 22px; width: 538px">
                        <asp:Image ID="Image1" runat="server" />
                    </td>
                    <td style="height: 22px">
                        <table class="nav-justified" style="height: 149px">
                            <tr>
                                <td>
                                    <table class="nav-justified">
                                        <tr>
                                            <td>
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td style="width: 97px">First axis:</td>
                                                        <td>
                                                            <asp:DropDownList ID="evalFirstAxisDDList" runat="server" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 97px">Second axis:</td>
                                                        <td>
                                                            <asp:DropDownList ID="evalSecondAxisDDList" runat="server" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:Button ID="evalGenerateFunctionButton" runat="server" Height="26px" Text="Generate function" Width="129px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="evalErrorLabel" runat="server" ForeColor="Red" Text="ERROR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="height: 400px">
                                    <table class="nav-justified">
                                        <tr>
                                            <td>Default values for numeric options:</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 350px">
                                                <asp:Panel ID="Panel1" runat="server" Height="340px">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table class="nav-justified">
                <tr>
                    <td style="text-decoration: underline; width: 80px;">Rotation:</td>
                    <td style="text-decoration: underline; width: 15px;">X:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="evalXRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="text-decoration: underline; width: 15px">Y:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="evalYRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="text-decoration: underline; width: 15px;">Z:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="evalZRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="width: 70px">
                        <asp:Button ID="evalRotateButton" runat="server" Text="Rotate" />
                    </td>
                    <td>
                        <asp:Label ID="evalRotateErrorLabel" runat="server" ForeColor="Red" Text="ERROR"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<p style="width: 943px">

    </p>
<p style="width: 943px; text-decoration: underline">

    <strong>Interactions and Influences:</strong></p>
<table class="nav-justified">
    <tr>
        <td style="text-decoration: underline">Interactions:</td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="interactionsTextBox" runat="server" Width="769px" Height="307px" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
            <table class="nav-justified">
                <tr>
                    <td style="width: 423px; text-decoration: underline; height: 22px">In how many interactions do the possible variables occur?</td>
                    <td style="text-decoration: underline; height: 22px">What is the abstract influence of each variable?</td>
                </tr>
                <tr>
                    <td style="width: 423px">
                        <asp:Chart ID="interactionChart" runat="server" Width="400px">
                            <series>
                                <asp:Series Name="Series1">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                    </td>
                    <td>
                        <asp:Chart ID="constantsChart" runat="server" Width="400px">
                            <series>
                                <asp:Series Name="Series1">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                    </td>
                </tr>
                <tr>
                    <td style="width: 423px; text-decoration: underline">What is the maximum abstract influence of each variable?</td>
                    <td style="text-decoration: underline">What is the maximum abstract influence of each variable depended on its usage in the configurations?</td>
                </tr>
                <tr>
                    <td style="width: 423px">
                        <asp:Chart ID="maxChart" runat="server" Width="400px">
                            <series>
                                <asp:Series Name="Series1">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                    </td>
                    <td>
                        <asp:Chart ID="maxOccuranceChart" runat="server" Width="400px">
                            <series>
                                <asp:Series Name="Series1">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                    </td>
                </tr>
                <tr>
                    <td style="width: 423px; text-decoration: underline">What is the influence range of each variable?</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 423px">
                        <asp:Chart ID="rangeChart" runat="server" Width="400px">
                            <series>
                                <asp:Series Name="Series1">
                                </asp:Series>
                            </series>
                            <chartareas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </chartareas>
                        </asp:Chart>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<p style="width: 943px">

    </p>
<p style="width: 943px; text-decoration: underline;">

    <strong>Measurements:</strong></p>
    <table class="nav-justified">
        <tr>
            <td>
                <table class="nav-justified">
                    <tr>
                        <td style="width: 55px">NFP:</td>
                        <td>
                            <asp:DropDownList ID="measureNFPDDList" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <table class="nav-justified">
                    <tr>
                        <td>Both graphs:</td>
                        <td>Measurements only:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Image2" runat="server" />
                        </td>
                        <td>
                            <asp:Image ID="Image5" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Absolute difference:</td>
                        <td>Relative difference:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Image3" runat="server" />
                        </td>
                        <td>
                            <asp:Image ID="Image4" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
            <table class="nav-justified">
                <tr>
                    <td style="text-decoration: underline; width: 80px;">Rotation:</td>
                    <td style="text-decoration: underline; width: 15px;">X:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="measureXRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="text-decoration: underline; width: 15px">Y:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="measureYRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="text-decoration: underline; width: 15px;">Z:</td>
                    <td style="width: 50px">
                        <asp:TextBox ID="measureZRotTextBox" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td style="width: 70px">
                        <asp:Button ID="measureRotateButton" runat="server" Text="Rotate" />
                    </td>
                    <td>
                        <asp:Label ID="measureRotateErrorLabel" runat="server" ForeColor="Red" Text="ERROR"></asp:Label>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    </asp:Content>
