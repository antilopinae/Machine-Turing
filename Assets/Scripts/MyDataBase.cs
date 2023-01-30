using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;

static class MyDataBase
{
    private const string fileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static MyDataBase()
    {
        DBPath = GetDatabasePath();
    }

    /// <summary> ���������� ���� � ��. ���� � ��� � ������ ����� �� ��������, �� �������� � � ��������� apk �����. </summary>
    private static string GetDatabasePath()
    {
#if UNITY_EDITOR
    return Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
    string filePath = Path.Combine(Application.persistentDataPath, fileName);
    if (!File.Exists(filePath)) UnpackDatabase(filePath);
    return filePath;
#elif UNITY_STANDALONE
    string filePath = Path.Combine(Application.dataPath, fileName);
    if(!File.Exists(filePath)) UnpackDatabase(filePath);
    return filePath;
#endif
    }

    /// <summary> ������������� ���� ������ � ��������� ����. </summary>
    /// <param name="toPath"> ���� � ������� ����� ����������� ���� ������. </param>
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }
    /// <summary> ���� ����� ��������� ����������� � ��. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> ���� ����� ��������� ����������� � ��. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> ���� ����� ��������� ������ query. </summary>
    /// <param name="query"> ���������� ������. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary> ���� ����� ��������� ������ query � ���������� ����� �������. </summary>
    /// <param name="query"> ���������� ������. </param>
    /// <returns> ���������� �������� 1 ������ 1 �������, ���� ��� �������. </returns>
    public static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    /// <summary> ���� ����� ���������� �������, ������� �������� ����������� ������� ������� query. </summary>
    /// <param name="query"> ���������� ������. </param>
    public static System.Data.DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }
}