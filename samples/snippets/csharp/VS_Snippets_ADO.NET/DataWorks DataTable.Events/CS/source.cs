using System;
using System.Data;

static class Class1
{
    static void Main()
    {
        DataTableEvents();
    }
    // <Snippet1>
    static void DataTableEvents()
    {
        DataTable table = new("Customers");
        // Add two columns, id and name.
        table.Columns.Add("id", typeof(int));
        table.Columns.Add("name", typeof(string));

        // Set the primary key.
        table.Columns["id"].Unique = true;
        table.PrimaryKey = [table.Columns["id"]];

        // Add a RowChanged event handler.
        table.RowChanged += Row_Changed;

        // Add a RowChanging event handler.
        table.RowChanging += Row_Changing;

        // Add a RowDeleted event handler.
        table.RowDeleted += Row_Deleted;

        // Add a RowDeleting event handler.
        table.RowDeleting += Row_Deleting;

        // Add a ColumnChanged event handler.
        table.ColumnChanged += Column_Changed;

        // Add a ColumnChanging event handler.
        table.ColumnChanging += Column_Changing;

        // Add a TableNewRow event handler.
        table.TableNewRow += Table_NewRow;

        // Add a TableCleared event handler.
        table.TableCleared += Table_Cleared;

        // Add a TableClearing event handler.
        table.TableClearing += Table_Clearing;

        // Add a customer.
        DataRow row = table.NewRow();
        row["id"] = 1;
        row["name"] = "Customer1";
        table.Rows.Add(row);

        table.AcceptChanges();

        // Change the customer name.
        table.Rows[0]["name"] = "ChangedCustomer1";

        // Delete the row.
        table.Rows[0].Delete();

        // Clear the table.
        table.Clear();
    }

    static void Row_Changed(object sender, DataRowChangeEventArgs e) =>
        Console.WriteLine($"Row_Changed Event: name={e.Row["name"]}; action={e.Action}");

    static void Row_Changing(object sender, DataRowChangeEventArgs e) =>
        Console.WriteLine($"Row_Changing Event: name={e.Row["name"]}; action={e.Action}");

    static void Row_Deleted(object sender, DataRowChangeEventArgs e) =>
        Console.WriteLine($"Row_Deleted Event: name={e.Row["name", DataRowVersion.Original]}; action={e.Action}");

    static void Row_Deleting(object sender,
    DataRowChangeEventArgs e) =>
        Console.WriteLine($"Row_Deleting Event: name={e.Row["name"]}; action={e.Action}");

    static void Column_Changed(object sender, DataColumnChangeEventArgs e) =>
        Console.WriteLine($"Column_Changed Event: ColumnName={e.Column.ColumnName}; RowState={e.Row.RowState}");

    static void Column_Changing(object sender, DataColumnChangeEventArgs e) =>
        Console.WriteLine($"Column_Changing Event: ColumnName={e.Column.ColumnName}; RowState={e.Row.RowState}");

    static void Table_NewRow(object sender,
        DataTableNewRowEventArgs e) =>
        Console.WriteLine($"Table_NewRow Event: RowState={e.Row.RowState.ToString()}");

    static void Table_Cleared(object sender, DataTableClearEventArgs e) =>
        Console.WriteLine("Table_Cleared Event: TableName={0}; Rows={1}",
            e.TableName, e.Table.Rows.Count.ToString());

    static void Table_Clearing(object sender, DataTableClearEventArgs e) =>
        Console.WriteLine("Table_Clearing Event: TableName={0}; Rows={1}",
            e.TableName, e.Table.Rows.Count.ToString());
    // </Snippet1>
}
