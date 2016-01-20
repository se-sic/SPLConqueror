<%@ Page Title="Function Learning" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FunctionLearning.aspx.cs" Inherits="SPLConqueror_WebSite.FunctionLearning" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <table class="nav-justified">
        <tr>
            <td style="width: 360px">
                <asp:FileUpload ID="modelFileUpload" runat="server" Width="315px" />
            </td>
            <td style="width: 525px">
                <asp:FileUpload ID="measurementsFileUpload" runat="server" Width="315px" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 360px">
                <asp:Button ID="loadModelButton" runat="server" Text="Read Variability Model" Width="163px" OnClick="loadModelButton_Click" />
            </td>
            <td style="width: 525px">
                <asp:Button ID="loadMeasurementsButton" runat="server" Text="Read Measurements" Width="155px" OnClick="loadMeasurementsButton_Click" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 360px">
                <asp:Label ID="modelErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="Please load a model."></asp:Label>
            </td>
            <td style="width: 525px">
                <asp:Label ID="measurementsErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="Please load some measurements."></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <p style="height: 25px">&nbsp;</p>
    <table class="nav-justified">
        <tr>
            <td>
                <asp:UpdatePanel ID="settingsUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="nav-justified">
                            <tr>
                                <td style="background-color: #C0C0C0; border-right-style: double;">
                                    <table class="nav-justified" style="width: 99%">
                                        <tr>
                                            <td style="text-decoration: underline"><em>NFP:</em></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 240px">
                                                <asp:CheckBoxList ID="nfpCheckBoxList" runat="server" Height="226px" Width="131px" OnSelectedIndexChanged="nfpCheckBoxList_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="cleanSamplingButton" runat="server" Text="Clean sampling" Width="113px" OnClick="cleanSamplingButton_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="background-color: #C0C0C0; border-right-style: double;">
                                    <table class="nav-justified">
                                        <tr>
                                            <td style="text-decoration: underline"><em>Binary Options: Sampling Heuristics</em></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td style="background-color: #C0C0C0; width: 21px;">
                                                            <asp:CheckBox ID="bhOptionCheckBox" runat="server" Text=" " />
                                                        </td>
                                                        <td style="background-color: #C0C0C0">Option-Wise</td>
                                                        <td style="background-color: #C0C0C0; width: 21px;">
                                                            <asp:CheckBox ID="bhPairCheckBox" runat="server" Text=" " />
                                                        </td>
                                                        <td style="background-color: #C0C0C0">
                                                            <asp:Label ID="Label18" runat="server" Text="Pair-Wise"></asp:Label>
                                                        </td>
                                                        <td style="background-color: #C0C0C0; width: 21px;">
                                                            <asp:CheckBox ID="bhNegOptionCheckBox" runat="server" Text=" " />
                                                        </td>
                                                        <td style="background-color: #C0C0C0">
                                                            <asp:Label ID="Label19" runat="server" Text="Negative Option-Wise"></asp:Label>
                                                        </td>
                                                        <td style="background-color: #C0C0C0; width: 21px;">
                                                            <asp:CheckBox ID="bhPopulationCheckBox" runat="server" Text=" " />
                                                        </td>
                                                        <td style="background-color: #C0C0C0">
                                                            <asp:Label ID="Label20" runat="server" Text="Whole Population"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px"></td>
                                            <td style="height: 10px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="text-decoration: underline"><em>Numeric Options: Numeric Designs</em></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #C0C0C0">
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td>
                                                            <table class="nav-justified">
                                                                <tr>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndBehnkenCheckBox" runat="server" Text=" " />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ndBehnkenLabel" runat="server" Text="BoxBehnken"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndCentralCompositeCheckBox" runat="server" Text=" " />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ndCentralCompositeLabel" runat="server" Text="CentralComposite"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndFullFactorialCheckBox" runat="server" Text=" " />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="ndFullFactorialLabel" runat="server" Text="Full-Factorial"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table class="nav-justified">
                                                                <tr>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndDOptimalCheckBox" runat="server" Text=" " OnCheckedChanged="ndDOptimalCheckBox_CheckedChanged" />
                                                                    </td>
                                                                    <td style="width: 160px">
                                                                        <asp:Label ID="ndDOptimalLabel" runat="server" Text="D-Optimal (k-Exchange)"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndDOptimalNLabel" runat="server" Text="n:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 75px">
                                                                        <asp:TextBox ID="ndDOptimalNTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">50</asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndDOptimalKLabel" runat="server" Text="k:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="ndDOptimalKTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">5</asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndPlackettBurmanCheckBox" runat="server" Text=" " OnCheckedChanged="ndPlackettBurmanCheckBox_CheckedChanged" />
                                                                    </td>
                                                                    <td style="width: 160px">
                                                                        <asp:Label ID="ndPlackettBurmanLabel" runat="server" Text="Plackett-Burman"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndPlackettBurmanLevelLabel" runat="server" Text="level:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 75px">
                                                                        <asp:TextBox ID="ndPlackettBurmanLevelTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">3</asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndPlackettBurmanNLabel" runat="server" Text="n:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="ndPlackettBurmanNTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">9</asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 21px">
                                                                        <asp:CheckBox ID="ndRandomSamplingCheckBox" runat="server" Text=" " OnCheckedChanged="ndRandomSamplingCheckBox_CheckedChanged" />
                                                                    </td>
                                                                    <td style="width: 160px">
                                                                        <asp:Label ID="ndRandomSamplingLabel" runat="server" Text="Random sampling"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndRandomSamplingNLabel" runat="server" Text="n:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 75px">
                                                                        <asp:TextBox ID="ndRandomSamplingNTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">100</asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 30px">
                                                                        <asp:Label ID="ndRandomSamplingSeedLabel" runat="server" Text="seed:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="ndRandomSamplingSeedTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">0</asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table class="nav-justified">
                                                                <tr>
                                                                    <td style="background-color: #C0C0C0; width: 21px;">
                                                                        <asp:CheckBox ID="ndHyperSamplingCheckBox" runat="server" Text=" " OnCheckedChanged="ndHyperSamplingCheckBox_CheckedChanged" />
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 160px;">
                                                                        <asp:Label ID="ndHyperSamplingLabel" runat="server" Text="Hyper sampling"></asp:Label>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 60px;">
                                                                        <asp:Label ID="ndHyperSamplingPercentLabel" runat="server" Text="percent:"></asp:Label>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 71px;">
                                                                        <asp:TextBox ID="ndHyperSamplingPercentTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">10</asp:TextBox>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: #C0C0C0; width: 21px;">
                                                                        <asp:CheckBox ID="ndOneFactorCheckBox" runat="server" Text=" " OnCheckedChanged="ndOneFactorCheckBox_CheckedChanged" />
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 160px;">
                                                                        <asp:Label ID="ndOneFactorLabel" runat="server" Text="One Factor at a Time"></asp:Label>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 60px;">
                                                                        <asp:Label ID="ndOneFactorValuesLabel" runat="server" Text="values:"></asp:Label>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0; width: 71px;">
                                                                        <asp:TextBox ID="ndOneFactorValuesTextBox" runat="server" Width="55px" Style="text-align: right" Enabled="False">5</asp:TextBox>
                                                                    </td>
                                                                    <td style="background-color: #C0C0C0">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px"></td>
                                            <td style="height: 10px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="text-right">
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td class="text-left" style="width: 377px">
                                                            <asp:Label ID="learningErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="An error ocurred. Check your parameters!" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="startLearningButton" runat="server" Text="Start Learning" Width="110px" OnClick="startLearningButton_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="text-right">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="background-color: #C0C0C0">
                                    <table class="nav-justified" style="height: 290px">
                                        <tr>
                                            <td style="height: 20px; text-decoration: underline;"><em>MachineLearning Settings:</em></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="mlSettingsPanel" runat="server" Height="265px" ScrollBars="Vertical" Width="245px">
                                                    <asp:Table ID="mlSettingsTable" runat="server">
                                                    </asp:Table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="loadMeasurementsButton" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ndDOptimalCheckBox" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ndPlackettBurmanCheckBox" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ndRandomSamplingCheckBox" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ndHyperSamplingCheckBox" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ndOneFactorCheckBox" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="startLearningButton" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 21px"></td>
        </tr>
        <tr>
            <td style="text-decoration: underline"><strong>Performance-Influence Model</strong></td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="dataUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="dataGridView" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="false" Width="943px">
                            <AlternatingRowStyle BackColor="Gainsboro" />
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#000065" />
                            <Columns>
                                <asp:BoundField DataField="Round" HeaderText="Round" />
                                <asp:BoundField DataField="Learning error" HeaderText="Learning error" />
                                <asp:BoundField DataField="Global error" HeaderText="Global error" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cleanSamplingButton" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="startLearningButton" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <span style="text-decoration: underline"><strong>Log</strong></span></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="logTextBox" runat="server" Height="100px" ReadOnly="True" TextMode="MultiLine" Width="931px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
