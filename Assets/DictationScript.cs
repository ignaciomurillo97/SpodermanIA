using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine.UI;
using System.Threading;
using UnityEditor;
using SpeechLib;
using System.Xml;
using System.IO;

public class DictationScript : MonoBehaviour
{
    public GridNodes Grid;
    //Keywords codes
    //1 - gridX number
    //2 - gridY number
    //3 - starting Position X
    //4 - starting Position Y
    //5 - starting Position X
    //6 - starting Position Y
    //7 - building Size value
    private ArrayList inputs;
    private DictationRecognizer dictationRecognizer;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private SpVoice voice;
    private int recognizedWordValue;
    private int gridX;
    private int gridY;
    private int positionX;
    private int positionY;
    private int targetX;
    private int targetY;
    private int buildingSize;
    private float obstacleRatio;
    private bool diagonalsAllowed;
    private String invalidEntry = "Invalid Entry, please try again";

    void Awake()
    {
        Grid = GetComponent<GridNodes>();
    }

    // Use this for initialization
    void Start()
    {
        voice = new SpVoice();
        voice.Volume = 100; // Volume (no xml)
        voice.Rate = 0;  // Rate (no xml)
        actions.Add("Set Grid Width", MatrixX);
        actions.Add("Set Grid Lenght", MatrixY);
        actions.Add("Set Position X", PositionX);
        actions.Add("Set Position Y", PositionY);
        actions.Add("Set Target X", TargetX);
        actions.Add("Set Target Y", TargetY);
        actions.Add("Set Building Size", BuildingSize);
        actions.Add("Set Obstacle Percentage", ObstaclePercetage);
        actions.Add("Print", Print);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray()); ;
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds = 20;  
        dictationRecognizer.AutoSilenceTimeoutSeconds = 1;  
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;   
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        UnityEngine.Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void MatrixX()
    {
        recognizedWordValue = 1;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter grid width value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void MatrixY()
    {
        recognizedWordValue = 2;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter grid lenght value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void PositionX()
    {
        recognizedWordValue = 3;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter position X value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void PositionY()
    {
        recognizedWordValue = 4;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter position Y value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void TargetX()
    {
        recognizedWordValue = 5;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter target X value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void TargetY()
    {
        recognizedWordValue = 6;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter target Y value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void BuildingSize()
    {
        recognizedWordValue = 7;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter building size value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void ObstaclePercetage()
    {
        recognizedWordValue = 8;
        EditorApplication.Beep();
        keywordRecognizer.Stop();
        PhraseRecognitionSystem.Shutdown();
        voice.Speak("Please enter obstacle percentage value", SpeechVoiceSpeakFlags.SVSFlagsAsync);
        dictationRecognizer.Start();
        dictationRecognizer.Stop();
        //recognizedWordValue = 0;
    }

    private void Print()
    {
        UnityEngine.Debug.Log(this.gridX);
        UnityEngine.Debug.Log(this.gridY);
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)   //handles recognized text

    {
        if (recognizedWordValue == 1)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                Grid.GridSizeX = result;
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 2)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                Grid.GridSizeY = result;
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 3)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                //Grid.GridSizeX = result;
                //Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 4)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                //this.positionY = result;
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 5)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                //this.targetX = result;
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 6)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                //this.targetY = result;
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 7)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                Grid.NodeSize = result;
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

        if (recognizedWordValue == 8)
        {
            try
            {
                int result = Int32.Parse(text);
                UnityEngine.Debug.Log(text);
                Grid.ObstacleProbability = (float)result / 100;
                Grid.InitData();
            }
            catch (Exception ex)
            {
                EditorApplication.Beep();
                voice.Speak(invalidEntry, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            finally
            {

            }
        }

    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // 
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)  //Handles after-recognition
    {
        dictationRecognizer.Stop();   
        PhraseRecognitionSystem.Restart();  //Starts phrase recognition again
        keywordRecognizer.Start();
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        
    }

    void OnDestroy() //Destroyer
    {
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        dictationRecognizer.Dispose();  
    }

}
