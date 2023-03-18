using UnityEngine;
using UnityEngine.UI;

namespace PersistentStorage.Example
{
    public class Example : MonoBehaviour
    {
        //[SerializeField] private PlayerStorageObject playerStorageObject;
        [SerializeField] private PlayerStorageObject.PlayerData defaultData;

        [SerializeField] private PlayerView playerView;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private PlayerStorageObject _playerStorageObject;
        
        private void Awake()
        {
            saveButton.onClick.AddListener(OnSaveClicked);
            loadButton.onClick.AddListener(OnLoadClicked);
            
            _playerStorageObject = PersistentStorage.Load<PlayerStorageObject, PlayerStorageObject.PlayerData>
                (
                    new PlayerStorageObject
                    (
                        defaultData, 
                        playerView.Initialize, 
                        playerView.Parse
                    )
                );
        }
        
        private void OnSaveClicked()
        {
            PersistentStorage.Save<PlayerStorageObject, PlayerStorageObject.PlayerData>(_playerStorageObject);
        }

        private void OnLoadClicked()
        {
            PersistentStorage.Load<PlayerStorageObject, PlayerStorageObject.PlayerData>(_playerStorageObject);
        }
    }
}