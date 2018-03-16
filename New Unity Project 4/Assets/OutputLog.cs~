using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

#if UNITY_UWP
using Windows.Storage;
using System.Threading.Tasks;
#endif

public class OutputLog : MonoBehaviour {


    DateTime time;

    private string message_me = "";					//書き出したいデータをためるstring。
	private string message_path = "";
	private string message_area = "";
	private string message_time = "";
	private string message_trajectory = "";
	private int interval_me = 0;					//データを書き出す間隔
	private int interval_path = 0;
	private int interval_area = 0;
	private int interval_time = 0;
	private int interval_trajectory = 0;
	//private bool newLogging = true;
	#if UNITY_UWP
    private StorageFolder folder_me;
    private StorageFile file_me;
    private Stream stream_me;
	private StorageFolder folder_path;
	private StorageFile file_path;
	private Stream stream_path;
	private StorageFolder folder_area;
	private StorageFile file_area;
	private Stream stream_area;
	private StorageFolder folder_time;
	private StorageFile file_time;
	private Stream stream_time;
	private StorageFolder folder_trajectory;
	private StorageFile file_trajectory;
	private Stream stream_trajectory;
	#endif


    
	/*
	 * ファイル出力のinit
	 */
	// Use this for initialization
	void Start () {
#if UNITY_UWP
        time = DateTime.Now;
        setupFileAsync();
#endif
    }

	/*
	 * それぞれの個数分データが蓄積されたときデータが書き出しされる
	 */
	// Update is called once per frame
	void Update () {
		

#if UNITY_UWP
        if (interval_me > 10)
        {
            output_me(message_me);
			message_me = "";
            interval_me = 0;
		}
		if (interval_path > 50000)
		{
			output_path(message_path);
			message_path = "";
			interval_path = 0;
		}
		if (interval_area > 1000000)
		{
			output_area(message_area);
			message_area = "";
			interval_area = 0;
		}
		if (interval_time > 100000)
		{
			output_time(message_time);
			message_time = "";
			interval_time = 0;
		}
		if (interval_trajectory > 200)
		{
			output_trajectory(message_trajectory);
			message_trajectory = "";
			interval_trajectory = 0;
		}
#endif

    }



	/*
	 * それぞれ書き出したいデータをstringに重ねる命令。message_に書き出すメッセージが入る
	 */
	public void writeMessage_me(string m){
		message_me += DateTime.Now.ToLocalTime()+","+m +"\n";
		interval_me++;
	}

	public void writeMessage_path(string m){
		message_path += DateTime.Now.ToLocalTime()+","+m +"\n";
		interval_path++;
	}

	public void writeMessage_area(string m){
		message_area += DateTime.Now.ToLocalTime()+","+m +"\n";
		interval_area++;
	}

	public void writeMessage_time(string m){
		message_time += DateTime.Now.ToLocalTime()+","+m +"\n";
		interval_time++;
	}

	public void writeMessage_trajectory(string m){
		message_trajectory += DateTime.Now.ToLocalTime()+","+m +"\n";
		interval_trajectory++;
	}

	/*
	 * それぞれのデータをファイル出力するスレッドを回す命令
	 */
	void output_me(string message)
	{
		#if UNITY_UWP
		Task.Run(async () =>
		{
		{
		var bytes = System.Text.Encoding.UTF8.GetBytes(message);
		await stream_me.WriteAsync(bytes, 0, bytes.Length);

		}
		});
		#endif
	}

	void output_path(string message)
	{
		#if UNITY_UWP
		Task.Run(async () =>
		{
		{
		var bytes = System.Text.Encoding.UTF8.GetBytes(message);
		await stream_path.WriteAsync(bytes, 0, bytes.Length);

		}
		});
		#endif
	}
	void output_area(string message)
	{
		#if UNITY_UWP
		Task.Run(async () =>
		{
		{
		var bytes = System.Text.Encoding.UTF8.GetBytes(message);
		await stream_area.WriteAsync(bytes, 0, bytes.Length);

		}
		});
		#endif
	}

	void output_time(string message)
	{
		#if UNITY_UWP
		Task.Run(async () =>
		{
		{
		var bytes = System.Text.Encoding.UTF8.GetBytes(message);
		await stream_time.WriteAsync(bytes, 0, bytes.Length);

		}
		});
		#endif
	}

	void output_trajectory(string message)
	{
		#if UNITY_UWP
		Task.Run(async () =>
		{
		{
		var bytes = System.Text.Encoding.UTF8.GetBytes(message);
		await stream_trajectory.WriteAsync(bytes, 0, bytes.Length);

		}
		});
		#endif
	}

	/*
	 * ファイル出力のセットアップ命令
	 */
#if UNITY_UWP
    async Task setupFileAsync()
    {
        folder_me = await ApplicationData.Current.LocalFolder.CreateFolderAsync("DocumentLibraryTest", CreationCollisionOption.OpenIfExists);
        file_me = await folder_me.CreateFileAsync(DateTime.Now.Year+"_"+ DateTime.Now.Month+ "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second  + "_me.txt", CreationCollisionOption.GenerateUniqueName);
        stream_me = await file_me.OpenStreamForWriteAsync();

		folder_path = await ApplicationData.Current.LocalFolder.CreateFolderAsync("DocumentLibraryTest", CreationCollisionOption.OpenIfExists);
		file_path = await folder_path.CreateFileAsync(DateTime.Now.Year+"_"+ DateTime.Now.Month+ "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second  + "_path.txt", CreationCollisionOption.GenerateUniqueName);
		stream_path = await file_path.OpenStreamForWriteAsync();

		folder_area = await ApplicationData.Current.LocalFolder.CreateFolderAsync("DocumentLibraryTest", CreationCollisionOption.OpenIfExists);
		file_area = await folder_area.CreateFileAsync(DateTime.Now.Year+"_"+ DateTime.Now.Month+ "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second  + "_area.txt", CreationCollisionOption.GenerateUniqueName);
		stream_area = await file_area.OpenStreamForWriteAsync();
        
		folder_time = await ApplicationData.Current.LocalFolder.CreateFolderAsync("DocumentLibraryTest", CreationCollisionOption.OpenIfExists);
		file_time = await folder_time.CreateFileAsync(DateTime.Now.Year+"_"+ DateTime.Now.Month+ "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second  + "_time.txt", CreationCollisionOption.GenerateUniqueName);
		stream_time = await file_time.OpenStreamForWriteAsync();

		folder_trajectory = await ApplicationData.Current.LocalFolder.CreateFolderAsync("DocumentLibrary", CreationCollisionOption.OpenIfExists);
		file_trajectory = await folder_trajectory.CreateFileAsync(DateTime.Now.Year+"_"+ DateTime.Now.Month+ "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second  + "_trajectory.txt", CreationCollisionOption.GenerateUniqueName);
		stream_trajectory = await file_trajectory.OpenStreamForWriteAsync();

    }
#endif
    /*
        public void WriteString(string s)
        {
    #if !UNITY_EDITOR && UNITY_METRO
            using (Stream stream = OpenFileForWrite(ApplicationData.Current.RoamingFolder.Path, "log.txt"))
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(s);
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
    #endif
        }


        private static Stream OpenFileForWrite(string folderName, string fileName)
        {
            Stream stream = null;
    #if !UNITY_EDITOR && UNITY_METRO
            Task task = new Task(
              async () => {
                  StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
                  StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                  stream = await file.OpenStreamForWriteAsync();
              });
            task.Start();
            task.Wait();
    #endif
            return stream;
        }
        */
}