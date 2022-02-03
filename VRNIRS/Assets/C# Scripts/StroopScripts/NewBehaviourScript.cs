using UnityEngine;
using System;
using System.IO;


public class ReadFile : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Read_fixed_trial(int niveau)
    {
        // Open the file explorer (to select the file)
        //string path = "C:/Users/juloi/Documents/GBM8870/fixed/test.txt";
        //string name_file;

        /*
      switch (niveau)
         {
             case 1:
                 name_file = "1";
                 break;
             case 2:
                 name_file = "2";
                 break;
             case 3:
                 name_file = "3";
                 break;

             default:
                 name_file = "4";
                 break;
         }
     */


        TextAsset txt = (TextAsset)Resources.Load("fixed_sequence", typeof(TextAsset));
        string allParameters = txt.text;

        int sequence = 1; //GLOBAL
        // Read the file
        if (true)
        {
            // Read all the parameters
            //string allParameters = File.ReadAllText(path);

            string[] parameters = allParameters.Split(';');


            Console.WriteLine(parameters[4]);
            if ((sequence - 1) < parameters.Length)
            {
                string ink_color = parameters[sequence].Split(',')[0];
                int ink_index; //global!
                string word_color = parameters[sequence].Split(',')[1];
                int word_index;
                string square_index = parameters[sequence].Split(',')[2];


                switch (ink_color)
                {
                    case "R":
                        ink_index = 1;
                        break;
                    case "G":
                        ink_index = 0;
                        break;
                    case "B":
                        ink_index = 2;
                        break;

                }

                switch (word_color)

                {
                    case "R":
                        word_index = 1;
                        break;
                    case "G":
                        word_index = 0;
                        break;
                    case "B":
                        word_index = 2;
                        break;

                }

            }







        }
    }
}
    