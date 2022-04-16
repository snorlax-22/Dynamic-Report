<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="DeTai4_DRP_N18DCCN020.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="scripts/bootstrap.css"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
        <div id="header">
                <asp:Label ID="LabelTitle" runat="server" Text="Nhập tiêu đề báo cáo: "></asp:Label>
                <asp:TextBox ID="TextBoxNhapTieuDe" runat="server"></asp:TextBox>    
        </div>
        <div id="main">
            <div id="table-content">
                <asp:Panel ID="PanelChonBang"  style="color:MidnightBlue; font-style:italic;" runat="server" Font-Bold="true" BackColor="White" ForeColor="Red" Height="250px">
                    <asp:Label ID="LabelChonBang" runat="server" Text="Chọn TABLE cần in báo cáo: "></asp:Label>
                    <br />
                    <asp:CheckBoxList ID="CheckBoxListTable" runat="server" Height="20px" OnSelectedIndexChanged="CheckBoxListTable_SelectedIndexChanged" Width="500px">
                    </asp:CheckBoxList>
                    <br />
                    
                </asp:Panel>
            </div>
        
            <div id="column-content">
                <asp:Panel ID="PanelChonCot" runat="server" BackColor="#99ccff" ForeColor="Maroon" Height="250px">

                    <br />
                    <asp:Label ID="LabelChonCot" runat="server" Text="Chọn COLUMN cần in báo cáo: "></asp:Label>
                    <br />
                 
                    <asp:CheckBoxList ID="CheckBoxListColumn" runat="server" Height="20px" OnSelectedIndexChanged="CheckBoxListColumn_SelectedIndexChanged" Width="500px">              
                    </asp:CheckBoxList>

                  
                   
                    <br />
                    <asp:Button class="btn btn-primary" ID="ButtonClearColumn" runat="server" OnClick="ButtonClearColumn_Click" Text="CLEAR ALL COLUMN" />
                    <br />
                    

                </asp:Panel>
            </div>

            <div id="query-content">
                <asp:Panel ID="PanelGridViewColumn" runat="server" BackColor="#ffff99" ForeColor="#6600cc" Height="300px">                
                    <br />
                    <asp:GridView ID="GridView1" runat="server" BackColor="White"  BorderColor="#CCCCCC"  BorderWidth="1px" CellPadding="3" Height="16px" >
                       <Columns>
                           <asp:TemplateField>
                               <HeaderTemplate>
                                   <div class="form-check form-switch">
                                       <input class="form-check-input" type="checkbox" id="flexSwitchCheckDefault">
                                   <asp:CheckBox class="form-check-label" ID="checkAll" runat="server" AutoPostBack="true" Text="Chọn tất cả"/>
                               </div>
                                       </HeaderTemplate>
                                   <ItemTemplate>
                                     <asp:CheckBox ID="ColumnChecked1" runat="server"/>
                                   </ItemTemplate>
                           </asp:TemplateField>
                  
                       
                    
                           <asp:TemplateField HeaderText="State" >
                               <ItemTemplate>
                                   <asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem Text="Non_Selected" Value=""></asp:ListItem>
                                        <asp:ListItem Text="SORT ASC" Value="ASC"></asp:ListItem>
                                        <asp:ListItem Text="SORT DESC" Value="DESC"></asp:ListItem>
                                        <asp:ListItem Text="COUNT" Value="COUNT"></asp:ListItem>
                                        <asp:ListItem Text="MIN" Value="MIN"></asp:ListItem>                                     
                                        <asp:ListItem Text="MAX" Value="MAX"></asp:ListItem>
                                   </asp:DropDownList>
                                </ItemTemplate>
                           </asp:TemplateField>           
              
                   
                           <asp:TemplateField HeaderText="Điều Kiện">
                               <ItemTemplate>
                                   <asp:TextBox ID="TextBoxDieuKien" runat="server"></asp:TextBox>
                               </ItemTemplate>
                           </asp:TemplateField>
                       </Columns>
                       <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                       <HeaderStyle BackColor="#006699" Font-Bold="true" ForeColor="White"/>
                       <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                       <RowStyle ForeColor="#000066"/>
                       <SelectedRowStyle BackColor="#006699" ForeColor="White" Font-Bold="true"/>
                       <SortedAscendingCellStyle BackColor="#000066"/>
                       <SortedAscendingHeaderStyle BackColor="#000066"/>
                       <SortedAscendingCellStyle BackColor="#007DBB"/>
                       <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                       <SortedDescendingHeaderStyle BackColor="#00547E"/>
                    </asp:GridView>


                    <br />
                    <asp:Label ID="lbltxt" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button class="btn btn-primary" ID="ButtonQuery" runat="server" OnClick="ButtonQuery_Click" Text="Tạo QUERY" />
                    <br />
                    <br />
                    <asp:Label ID="LabelMess" runat="server"></asp:Label>
                    <br />
                   
                </asp:Panel>
            </div>

            <div id="report-content">        
                <br />   
                <br />
                 <asp:Button ID="btnReport" runat="server" OnClick="ButtonReport_Click" Text="Tạo REPORT" />
                <br />
                <br />
            </div>
        </div>
</asp:Content>
