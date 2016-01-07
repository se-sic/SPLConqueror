<%@ Page Title="Model Editor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModelEditor.aspx.cs" Inherits="SPLConqueror_WebSite.ModelEditor" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.
    </h2>
    <h3>General view</h3>
    <p>
        <asp:Button ID="newModelButton" runat="server" Text="New Model" Width="84px" OnClick="newModelButton_Click" />
        </p>
    <asp:UpdatePanel ID="modelUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TreeView ID="modelTreeView" runat="server" Height="311px" OnSelectedNodeChanged="modelTreeView_SelectedNodeChanged" Width="574px">
            </asp:TreeView>
            <br />
            <table class="nav-justified" style="width: 15%">
                <tr>
                    <td>
                        <asp:Button ID="loadModelButton" runat="server" OnClick="loadModelButton_Click" Text="Load Model" Width="87px" />
                    </td>
                    <td>
                        <asp:FileUpload ID="modelFileUpload" runat="server" CssClass="col-md-offset-0" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="modelErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="ERROR" Visible="False"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="newModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="loadModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="modelTreeView" EventName="SelectedNodeChanged" />
            <asp:AsyncPostBackTrigger ControlID="creAddOptionButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editConfirmButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <h3>Option editor</h3>
    <h4>Create option</h4>
    <asp:UpdatePanel ID="optionCreationUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="nav-justified">
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 450px;">
                        <table class="nav-justified" style="width: 132%">
                            <tr>
                                <td style="width: 105px">Feature name:</td>
                                <td>
                                    <asp:TextBox ID="creFeatureNameTextBox" runat="server" Width="177px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 22px; width: 105px">Parent feature:</td>
                                <td style="height: 22px">
                                    <asp:DropDownList ID="creParentFeatureDDList" runat="server" Height="22px" Width="180px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 22px; width: 105px">
                                    <table class="nav-justified">
                                        <tr>
                                            <td style="height: 23px">Option type:</td>
                                        </tr>
                                        <tr>
                                            <td style="height: 30px"></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="height: 22px">
                                    <table class="nav-justified">
                                        <tr>
                                            <td style="border-width: 2px; width: 23px; border-left-style: solid; border-top-style: solid;">
                                                <asp:RadioButton ID="creBinaryRadioButton" runat="server" AutoPostBack="True" OnCheckedChanged="creBinaryRadioButton_CheckedChanged" Text=" " />
                                            </td>
                                            <td style="border-right-style: solid; border-right-width: 1px; border-top-style: solid; border-top-width: 2px;">Binary</td>
                                            <td style="border-top-style: solid; border-width: 2px">
                                                <asp:RadioButton ID="creNumericRadioButton" runat="server" AutoPostBack="True" OnCheckedChanged="creNumericRadioButton_CheckedChanged" Text=" " />
                                            </td>
                                            <td style="border-top-style: solid; border-right-style: solid; border-width: 2px">Numeric</td>
                                        </tr>
                                        <tr>
                                            <td style="border-width: 2px; width: 23px; border-left-style: solid;">&nbsp;</td>
                                            <td style="border-right-style: solid; border-right-width: 1px;">
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="creOptionalCheckBox" runat="server" Enabled="False" Text=" " />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="creOptionalLabel" runat="server" Enabled="False" Text="Optional"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 23px">&nbsp;</td>
                                            <td style="border-right-style: solid; border-width: 2px">
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="creRangeLabel" runat="server" Text="Range of values:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="creMinLabel" runat="server" Text="Min:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="creMinTextBox" runat="server" Enabled="False" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="creMaxLabel" runat="server" Text="Max:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="creMaxTextBox" runat="server" Enabled="False" Width="50px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-width: 2px; width: 23px; border-bottom-style: solid; border-left-style: solid;">&nbsp;</td>
                                            <td style="border-bottom-style: solid; border-right-width: 1px; border-bottom-width: 2px; border-right-style: solid;">&nbsp;</td>
                                            <td style="border-bottom-style: solid; border-width: 2px">&nbsp;</td>
                                            <td style="border-right-style: solid; border-bottom-style: solid; border-width: 2px">
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td style="width: 23px">
                                                            <asp:CheckBox ID="creStepSizeCheckBox" runat="server" AutoPostBack="True" Enabled="False" OnCheckedChanged="creStepSizeCheckBox_CheckedChanged" Text=" " />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="creStepSizeLabel" runat="server" Text="Step size:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="creStepSizeTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="creStepExampleLabel" runat="server" Text="(e.g. n + 1)"></asp:Label>
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
                    <td style="background-color: #FFFFFF; width: 224px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 450px;">
                        <table class="nav-justified" style="width: 133%">
                            <tr>
                                <td style="width: 30px">
                                    <asp:CheckBox ID="crePrePostCheckBox" runat="server" OnCheckedChanged="crePrePostCheckBox_CheckedChanged" Text=" " AutoPostBack="True" />
                                </td>
                                <td style="width: 50px">
                                    <asp:Label ID="crePreLabel" runat="server" Text="Prefix:"></asp:Label>
                                </td>
                                <td style="width: 145px">
                                    <asp:TextBox ID="crePreTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                </td>
                                <td style="width: 50px">
                                    <asp:Label ID="crePostLabel" runat="server" Text="Postfix:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="crePostTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 224px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 450px;">
                        <table class="nav-justified" style="width: 133%">
                            <tr>
                                <td style="width: 200px">Variant Generation Parameter:</td>
                                <td>
                                    <asp:TextBox ID="creVarGenParameterTextBox" runat="server" Enabled="False" Width="200px"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 224px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 450px;">
                        <table class="nav-justified" style="width: 133%">
                            <tr>
                                <td style="height: 50px">
                                    <asp:Button ID="creAddOptionButton" runat="server" OnClick="creAddOptionButton_Click" Text="Add Option" Width="100px" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="height: 25px">
                                    <asp:Label ID="creErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="ERROR" Visible="False"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 224px;">&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="newModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="loadModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="modelTreeView" EventName="SelectedNodeChanged" />
            <asp:AsyncPostBackTrigger ControlID="creBinaryRadioButton" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="creNumericRadioButton" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="creStepSizeCheckBox" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="crePrePostCheckBox" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="creAddOptionButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editConfirmButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />

    <h4>Edit options</h4>
    <asp:UpdatePanel ID="optionEditorUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="nav-justified">
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0"><strong><em>General Settings:</em></strong></td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0">
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 163px">Feature name:</td>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="editFeatureNameDDList" runat="server" Enabled="False" OnSelectedIndexChanged="editFeatureNameDDList_SelectedIndexChanged" Width="180px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="text-right" style="width: 75px">&nbsp;&nbsp;
                                    <asp:Label ID="Label3" runat="server" Text="Rename?"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="editRenameTextBox" runat="server" CssClass="col-md-offset-0" Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 163px">Parent of the feature:</td>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="editParentDDList" runat="server" Enabled="False" Width="180px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 75px" class="text-right">
                                    <asp:CheckBox ID="editOptionalCheckBox" runat="server" Text=" " />
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="editOptionalLabel" runat="server" Text="Optional"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; height: 10px; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; height: 10px;">&nbsp;</td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px; height: 22px;"></td>
                    <td style="background-color: #C0C0C0; height: 22px;"><em><strong>Option Settings:</strong></em></td>
                    <td style="background-color: #FFFFFF; width: 250px; height: 22px;"></td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0">
                        <table class="nav-justified" style="margin-top: 7px">
                            <tr>
                                <td style="border-left-style: solid; border-top-style: solid; width: 12px; border-left-width: 2px; border-right-width: 2px; border-top-width: 2px; border-bottom-width: 2px;">
                                    &nbsp;</td>
                                <td style="border-width: 2px; border-top-style: solid; border-right-style: solid;">
                                    <asp:Label ID="editNumericLabel" runat="server" Text="Numeric:" style="text-decoration: underline"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="editNumericErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="The selected feature is no numeric option." Visible="False"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="border-left-style: solid; border-bottom-style: solid; width: 12px; border-left-width: 2px; border-right-width: 2px; border-top-width: 2px; border-bottom-width: 2px;">
                                    &nbsp;</td>
                                <td style="border-width: 2px; width: 388px; border-bottom-style: solid; border-right-style: solid;">
                                    <table class="nav-justified">
                                        <tr>
                                            <td>
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td style="width: 112px">
                                                            <asp:Label ID="editRangeLabel" runat="server" Text="Range of values:"></asp:Label>
                                                        </td>
                                                        <td class="text-center" style="width: 43px">
                                                            <asp:Label ID="editMinLabel" runat="server" Text="Min:"></asp:Label>
                                                        </td>
                                                        <td style="width: 100px">
                                                            <asp:TextBox ID="editMinTextBox" runat="server" Enabled="False" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td class="text-center" style="width: 42px">
                                                            <asp:Label ID="editMaxLabel" runat="server" Text="Max:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="editMaxTextBox" runat="server" Enabled="False" Width="50px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="nav-justified">
                                                    <tr>
                                                        <td style="width: 71px">
                                                            <asp:Label ID="editStepSizeLabel" runat="server" Text="Step size:"></asp:Label>
                                                        </td>
                                                        <td style="width: 140px">
                                                            <asp:TextBox ID="editStepSizeTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="editStepExampleLabel" runat="server" Text="(e.g. n + 1)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; height: 10px; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; height: 10px;">&nbsp;</td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0"><strong><em>Other Settings:</em></strong></td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <table class="nav-justified">
                                        <tr>
                                            <td style="width: 50px">Prefix:</td>
                                            <td style="width: 178px">
                                                <asp:TextBox ID="editPreTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                            </td>
                                            <td style="width: 50px">Postfix:</td>
                                            <td>
                                                <asp:TextBox ID="editPostTextBox" runat="server" Enabled="False" Width="130px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="nav-justified">
                                        <tr>
                                            <td style="width: 228px">Variant Generation Parameter:</td>
                                            <td>
                                                <asp:TextBox ID="editVarGenParameterTextBox" runat="server" Enabled="False" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; height: 10px; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; height: 10px;">&nbsp;</td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0"><strong><em>Excludes und Implications:</em></strong></td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0">
                        <table class="nav-justified">
                            <tr>
                                <td><span style="text-decoration: underline">Excludes:</span></td>
                                <td><span style="text-decoration: underline">Implies:</span></td>
                            </tr>
                            <tr>
                                <td style="height: 24px">
                                    <asp:CheckBoxList ID="editExcludeCheckBoxList" runat="server" Enabled="False" Height="130px" OnSelectedIndexChanged="editExcludeCheckBoxList_SelectedIndexChanged" Width="400px">
                                    </asp:CheckBoxList>
                                </td>
                                <td style="height: 24px">
                                    <asp:CheckBoxList ID="editImplyCheckBoxList" runat="server" Enabled="False" Height="130px" OnSelectedIndexChanged="editImplyCheckBoxList_SelectedIndexChanged" Width="400px">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="editExcludeAddButton" runat="server" Enabled="False" OnClick="editExcludeAddButton_Click" Text="Add" />
                                </td>
                                <td>
                                    <asp:Button ID="editImplyAddButton" runat="server" Enabled="False" OnClick="editImplyAddButton_Click" Text="Add" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="editExcludeListBox" runat="server" Enabled="False" Height="180px" Width="400px"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:ListBox ID="editImplyListBox" runat="server" Enabled="False" Height="180px" Width="400px"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="editExcludeDeleteButton" runat="server" Enabled="False" OnClick="editExcludeDeleteButton_Click" Text="Delete" />
                                </td>
                                <td>
                                    <asp:Button ID="editImplyDeleteButton" runat="server" Enabled="False" OnClick="editImplyDeleteButton_Click" Text="Delete" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; height: 50px; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; height: 50px;">
                        <asp:Button ID="editConfirmButton" runat="server" OnClick="editConfirmButton_Click" Text="Confirm changes" Width="125px" />
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: #C0C0C0; width: 10px;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; ">
                        <asp:Label ID="editErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text="ERROR" Visible="False"></asp:Label>
                    </td>
                    <td style="background-color: #FFFFFF; width: 250px;">&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="newModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="loadModelButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="modelTreeView" EventName="SelectedNodeChanged" />
            <asp:AsyncPostBackTrigger ControlID="creAddOptionButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editFeatureNameDDList" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="editExcludeAddButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editExcludeDeleteButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editImplyAddButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editImplyDeleteButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="editConfirmButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <h3>Constraints editor</h3>
    <asp:UpdatePanel ID="constraintsUpdate" runat="server">
        <ContentTemplate>
            <table class="nav-justified" id="constraintsTable">
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0;">&nbsp;<td style="width: 433px; background-color: #C0C0C0;">
                        <h4>Complex Boolean Constraints
                            <td style="width: 15px; background-color: #C0C0C0;">&nbsp;<td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;<td style="background-color: #C0C0C0; width: 441px;">
                                <h4>Non-Boolean Constraints</h4>
                                </td>
                                <td style="background-color: #FFFFFF; ">&nbsp;</td>
                                </td>
                            </td>
                        </h4>
                        </td>
                </tr>
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 433px; background-color: #C0C0C0;">
                        <table class="nav-justified" style="width: 92%; height: 108px;">
                            <tr>
                                <td style="width: 65px; height: 28px;">
                                    <asp:Label ID="Label1" runat="server" Text="Option:"></asp:Label>
                                </td>
                                <td style="width: 226px; height: 28px;">
                                    <asp:DropDownList ID="boolOptionDDList" runat="server" Enabled="False" Height="22px" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 29px; height: 28px;"><strong>
                                    <asp:Button ID="boolAndButton" runat="server" Enabled="False" OnClick="boolAndButton_Click" style="font-weight: bold" Text="&amp;" Width="30px" />
                                    </strong></td>
                                <td style="width: 9px; height: 28px;"><strong>
                                    <asp:Button ID="boolOrButton" runat="server" Enabled="False" OnClick="boolOrButton_Click" style="font-weight: bold" Text="|" Width="30px" />
                                    </strong></td>
                            </tr>
                            <tr>
                                <td style="width: 65px">&nbsp;</td>
                                <td style="width: 226px">
                                    <asp:Button ID="boolAddOptionButton" runat="server" Enabled="False" OnClick="boolAddOptionButton_Click" Text="Add option" Width="85px" />
                                </td>
                                <td style="width: 29px"><strong>
                                    <asp:Button ID="boolImplButton" runat="server" Enabled="False" OnClick="boolImplButton_Click" style="font-weight: bold" Text="=&gt;" />
                                    </strong></td>
                                <td style="width: 9px"><strong>
                                    <asp:Button ID="boolNegButton" runat="server" OnClick="boolNegButton_Click" style="font-weight: bold" Text="!" Width="30px" />
                                    </strong></td>
                            </tr>
                            <tr>
                                <td style="width: 65px">&nbsp;</td>
                                <td style="width: 226px">&nbsp;</td>
                                <td style="width: 29px">&nbsp;</td>
                                <td style="width: 9px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 65px; height: 41px;"></td>
                                <td style="width: 226px; height: 41px;"></td>
                                <td style="width: 29px; height: 41px;"></td>
                                <td style="width: 9px; height: 41px;"></td>
                            </tr>
                            <tr>
                                <td style="width: 65px">&nbsp;</td>
                                <td style="width: 226px">&nbsp;</td>
                                <td style="width: 29px">&nbsp;</td>
                                <td style="width: 9px">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 15px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 441px;">
                        <table class="nav-justified">
                            <tr>
                                <td style="width: 56px">
                                    <asp:Label ID="Label2" runat="server" Text="Option:"></asp:Label>
                                </td>
                                <td style="width: 193px">
                                    <asp:DropDownList ID="nbOptionDDList" runat="server" Width="150px" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb7Button" runat="server" Text="7" Width="30px" style="font-weight: bold" OnClick="nb7Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb8Button" runat="server" Text="8" Width="30px" style="font-weight: bold" OnClick="nb8Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td><strong>
                                    <asp:Button ID="nb9Button" runat="server" Text="9" Width="30px" style="font-weight: bold" OnClick="nb9Button_Click" Enabled="False"/>
                                    </strong></td>
                            </tr>
                            <tr>
                                <td style="width: 56px">&nbsp;</td>
                                <td style="width: 193px">
                                    <asp:Button ID="nbAddOptionButton" runat="server" Text="Add option" Width="85px" OnClick="nbAddOptionButton_Click" Enabled="False" />
                                </td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb4Button" runat="server" Text="4" Width="30px" style="font-weight: bold" OnClick="nb4Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb5Button" runat="server" Text="5" Width="30px" style="font-weight: bold" OnClick="nb5Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td><strong>
                                    <asp:Button ID="nb6Button" runat="server" Text="6" Width="30px" style="font-weight: bold" OnClick="nb6Button_Click" Enabled="False"/>
                                    </strong></td>
                            </tr>
                            <tr>
                                <td style="width: 56px">&nbsp;</td>
                                <td style="width: 193px">&nbsp;</td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb1Button" runat="server" Text="1" Width="30px" style="font-weight: bold" OnClick="nb1Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb2Button" runat="server" Text="2" Width="30px" style="font-weight: bold" OnClick="nb2Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td><strong>
                                    <asp:Button ID="nb3Button" runat="server" Text="3" Width="30px" style="font-weight: bold" OnClick="nb3Button_Click" Enabled="False"/>
                                    </strong></td>
                            </tr>
                            <tr>
                                <td class="text-right" style="width: 56px"><strong>
                                    <asp:Button ID="nbPlusButton" runat="server" Text="+" Width="30px" style="font-weight: bold" OnClick="nbPlusButton_Click" Enabled="False" />
                                    </strong></td>
                                <td style="width: 193px"><strong>
                                    <asp:Button ID="nbSubButton" runat="server" Text="-" Width="30px" style="font-weight: bold" OnClick="nbSubButton_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px">&nbsp;</td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nb0Button" runat="server" Text="0" Width="30px" style="font-weight: bold" OnClick="nb0Button_Click" Enabled="False"/>
                                    </strong></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="text-right" style="width: 56px"><strong>
                                    <asp:Button ID="nbMulButton" runat="server" Text="*" Width="30px" style="font-weight: bold" OnClick="nbMulButton_Click" Enabled="False" />
                                    </strong></td>
                                <td style="width: 193px"><strong>
                                    <asp:Button ID="nbPointButton" runat="server" Text="." Width="30px" style="font-weight: bold" OnClick="nbPointButton_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nbEqButton" runat="server" Text="=" Width="30px" style="font-weight: bold" OnClick="nbEqButton_Click" Enabled="False"/>
                                    </strong></td>
                                <td style="width: 32px"><strong>
                                    <asp:Button ID="nbGreEqButton" runat="server" Text="&gt;=" Width="30px" style="font-weight: bold" OnClick="nbGreEqButton_Click" Enabled="False"/>
                                    </strong></td>
                                <td><strong>
                                    <asp:Button ID="nbGreButton" runat="server" Text="&gt;" Width="30px" style="font-weight: bold; height: 26px;" OnClick="nbGreButton_Click" Enabled="False"/>
                                    </strong></td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: #FFFFFF; ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 433px; background-color: #C0C0C0;">
                        <asp:TextBox ID="boolConstraintTextBox" runat="server" Enabled="False" ReadOnly="True" Width="400px"></asp:TextBox>
                    </td>
                    <td style="width: 15px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 441px;">
                        <asp:TextBox ID="nbConstraintTextBox" runat="server" Width="421px" ReadOnly="True" Enabled="False"></asp:TextBox>
                    </td>
                    <td style="background-color: #FFFFFF; ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0; ">&nbsp;</td>
                    <td style="width: 433px; background-color: #C0C0C0; height: 45px;">
                        <asp:Button ID="boolAddConstraintButton" runat="server" Enabled="False" OnClick="boolAddConstraintButton_Click" Text="Add" Width="40px" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="boolRemoveButton" runat="server" Enabled="False" OnClick="boolRemoveButton_Click" Text="Remove" Width="65px" />
                    </td>
                    <td style="width: 15px; background-color: #C0C0C0; "></td>
                    <td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 441px; height: 45px;">
                        <asp:Button ID="nbAddConstraintButton" runat="server" Text="Add" Width="40px" OnClick="nbAddConstraintButton_Click" Enabled="False" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="nbRemoveButton" runat="server" Text="Remove" Width="65px" OnClick="nbRemoveButton_Click" Enabled="False" />
                    </td>
                    <td style="background-color: #FFFFFF; "></td>
                </tr>
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 433px; background-color: #C0C0C0;">
                        <asp:ListBox ID="boolConstraintListBox" runat="server" Enabled="False" Height="190px" Width="400px"></asp:ListBox>
                    </td>
                    <td style="width: 15px; background-color: #C0C0C0;">&nbsp;</td>
                    <td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 441px;">
                        <asp:ListBox ID="nbConstraintListBox" runat="server" Height="190px" Width="421px" Enabled="False"></asp:ListBox>
                    </td>
                    <td style="background-color: #FFFFFF; ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10px; background-color: #C0C0C0; ">&nbsp;</td>
                    <td style="width: 433px; background-color: #C0C0C0; height: 45px;">
                        <asp:Button ID="boolDeleteButton" runat="server" Enabled="False" OnClick="boolDeleteButton_Click" Text="Delete" />
                    </td>
                    <td style="width: 15px; background-color: #C0C0C0; "></td>
                    <td style="width: 15px; background-color: #C0C0C0; border-left-style: double;">&nbsp;</td>
                    <td style="background-color: #C0C0C0; width: 441px; height: 45px;">
                        <asp:Button ID="nbDeleteButton" runat="server" Text="Delete" OnClick="nbDeleteButton_Click" Enabled="False" />
                    </td>
                    <td style="background-color: #FFFFFF; "></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        
    </script>
    </asp:Content>
