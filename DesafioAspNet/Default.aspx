<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DesafioAspNet._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CURD OPERATION</title>
  <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.1.5/dist/sweetalert2.all.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" integrity="sha512-z3gLpd7yknf1YoNbCzqRKc4qyor8gaKU1qmn+CShxbuBusANI9QpRohGBreCFkKxLhei6S9CQXFEbbKuqLg0DA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
        <script>
            function showAlert(title, message, type) {
                Swal.fire(title, message, type);
            }
        </script>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f1f1f1;
            /*background-image: url("https://www.transparenttextures.com/patterns/arches.png");*/
            padding: 20px;
        }

        .container {
            max-width: 360px;
            margin: auto;
            background-color: #fff;
            padding: 15px;
            border-radius: 5px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .table-atletas {
            width: 100%;
        }

        .table-atletas td {
            padding: 6px;
        }

        .table-atletas label {
            font-weight: bold;
        }

        .table-atletas .btn-submit {
            background-color: #007BFF;
            color: #fff;
            border: none;
            padding: 6px 10px;
            border-radius: 4px;
            cursor: pointer;
            height: 40px;
            font-size: medium;
        }

        .table-atletas .btn-submit:hover {
            background-color: #0056b3;
        }

        .table-atletas td{
            display: grid
        }

        .grid-view {
            margin-top: 30px;
        }

        .grid-view .fa-trash {
            color: #cc0000;
            cursor: pointer;
        }

        .grid-view table {
            width: 100%;
            border-collapse: collapse;
        }

        .grid-view th,
        .grid-view td {
            padding: 10px;
            border: 1px solid #ccc;
            text-align: left;
        }

        .grid-view th {
            background-color: #111518;
            color: #fff;
        }

        .edit-icon,
        .delete-icon,
        .update-icon,
        .cancel-icon {
            margin-right: 10px;
            text-decoration: none;
            outline: none !important;
        }

        .no-outline:focus {
            outline: none !important;
        }

        .no-highlight {
            background-color: transparent;
            border: none;
            font-size: 16PX;
        }

        .no-highlight:focus {
            border: none;
            outline: none !important;
        }
        .action-column {
            width: 95px;
        }

        .action-column a {
            margin-right: 5px; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Atletas</h2>
            <table class="table-atletas">
                <tr>
                    <td>
                        <label for="Nome">Nome:</label>
                        <asp:TextBox ID="Nome" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Apelido">Apelido:</label>
                        <asp:TextBox ID="Apelido" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Nascimento">Data de nascimento:</label>
                       <%-- <asp:Calendar ID="Nascimento" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray">
                           </OtherMonthDayStyle>

                           <TitleStyle BackColor="Blue"
                                       ForeColor="White">
                           </TitleStyle>

                           <DayStyle BackColor="gray">
                           </DayStyle>

                           <SelectedDayStyle BackColor="LightGray"
                                             Font-Bold="True">
                           </SelectedDayStyle>
                        </asp:Calendar>--%>
                        <asp:TextBox ID="Nascimento" runat="server" TextMode="Date" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Altura">Altura:</label>
                        <asp:TextBox ID="Altura" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Peso">Peso:</label>
                        <asp:TextBox ID="Peso" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Posicao">Posição:</label>
                        <asp:TextBox ID="Posicao" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Camisa">Camisa:</label>
                        <asp:TextBox ID="Camisa" runat="server" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn" runat="server" Text="Inserir" OnClick="Inserir" CssClass="btn-submit" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="grid-view">
            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" GridLines="Both" DataKeyNames="id" OnPageIndexChanging="GvData_PageIndexChanging" OnRowCancelingEdit="GvData_RowCancelingEdit" OnRowDeleting="GvData_RowDeleting" OnRowEditing="GvData_RowEditing" OnRowUpdating="Gvdata_RowUpdating">

                <Columns>
                    <asp:TemplateField HeaderText="Id" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtId" runat="server" Text='<%# Eval("Id") %>' ReadOnly="true" CssClass="no-highlight"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="nome" HeaderText="Nome" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="apelido" HeaderText="Apelido" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="nascimento" HeaderText="Data de Nascimento" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="altura" HeaderText="Altura" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="peso" HeaderText="Peso" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="posicao" HeaderText="Posição" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:BoundField DataField="camisa" HeaderText="Camisa" ItemStyle-BorderWidth="1px" ItemStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="1px" HeaderStyle-BorderStyle="Solid" />
                    <asp:TemplateField HeaderText="IMC">
                        <ItemTemplate>
                            <asp:Label ID="IMC" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Classificação IMC">
                        <ItemTemplate>
                            <asp:Label ID="classif" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="action-column">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="fa fa-pen-to-square edit-icon no-outline"></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="fa fa-trash delete-icon"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="fa fa-check update-icon"></asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="fa fa-xmark cancel-icon"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
