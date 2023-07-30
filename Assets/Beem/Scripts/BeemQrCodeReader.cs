using System;
using Qualcomm.Snapdragon.Spaces.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
public class BeemQrCodeReader : MonoBehaviour
{
    private int _framerate;
    private BarcodeReader _barReader;
    private  bool _decoding;
    
    [SerializeField] private CameraFrameAccessSampleController _cameraFrameAccessSampleController;
    [SerializeField] private Text _text;
    
    private void Awake()
    {
        _barReader = new BarcodeReader
        {
            AutoRotate = true,
            TryInverted = true
        };
    }

    private void Update()
    {
        if (_framerate++ % 15 != 0)
        {
            return;
        }
        
        if (!_cameraFrameAccessSampleController.LockTexture)
        {
            Loom.RunAsync(() =>
            {
                try
                {
                    var tex = _cameraFrameAccessSampleController.CameraRawImage.texture as Texture2D;
                    if (tex == null)
                    {
                        return;
                    }
                        
                    var data = _barReader.Decode(tex.GetPixels32(), tex.width, tex.height); //start decode
                    if (data == null)
                    {
                        return;
                    }
                        
                    _decoding = true;
                    _text.text = data.Text;
                    
                    PlayerPrefs.SetString("VideoName", data.Text);
                    SceneManager.LoadScene("Anchor Sample");
                }
                catch (Exception e)
                {
                    _text.text = "Decode Error: " + e.Data;
                    _decoding = false;
                }
            });	
        }
    }
}
