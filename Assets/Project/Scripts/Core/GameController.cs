using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// mettre la mort quand ca atteint tombe dans le vide ! v
// Ajouter 1 niveau v
// Ajouter Music v
// Best sprite handling v
// Ajouter bruitage (mort, win) v
// score v
// Fix score qui tremble v
// fix le score v
// Ajouter le son de mort quand on prend un degat v
// Ajouter passages secrets v
// COMPRENDRE POURQUOI CHUTE INFINIE SUR PLATFORM VERTICALE (IS GROUNDED FALSE ??) fixed v
// COMPRENDRE POURQUOI PEUT PAS SAUTER SI PLATFORM MONTE fixed v
// Pause (resume/retour menu) v
// Affichage score quand mort (score, replay, menu) v
// Gameplay couché v 
// ajouter joystick mobile => selectionner depuis le menu de faire un mode mobile ?
// GAMEPLAY : DEVENIR PETIT POUR PASSER DES OBSTACLES => NOPE
// meilleur facon de creer des levels (outils?) + en xml ??
// CONTINUE REFACTO => TOUT DOIT ETRE PREFAB
// MOVING PLATFORM & MOVING ENEMIES ??


public class GameController : MonoBehaviour
{
    [Header("Player")]
    GameObject _playerGO;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] float _jumpForce;
    [SerializeField] float _moveSpeed;

    [Header("Score")]
    GameObject _scoreGO;
    [SerializeField] Transform _scoreParent;
    [SerializeField] GameObject _scorePrefab;

    [Header("Life")]
    GameObject _lifeGO;
    [SerializeField] Transform _lifeParent;
    [SerializeField] GameObject _lifePrefab;
    [SerializeField] int _life;

    [Header("Collectible")]
    [SerializeField] Transform _collectibleParent;
    [SerializeField] GameObject _collectiblePrefab;
    List<GameObject> _collectibles;

    [SerializeField] CinemachineVirtualCamera _vcam;

    [SerializeField] GameObject _pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        InitiatePlayer();
        _vcam.Follow = _playerGO.transform;
        InitiateScore();
        InitiateLife();
        InitiateCollectibles();
    }

    void InitiatePlayer()
    {
        _playerGO = Instantiate<GameObject>(_playerPrefab);
        var player = _playerGO.GetComponent<Player>();
        player.Initiate(new Vector3(-21, 1, 0), _jumpForce, _moveSpeed);
        player.OnTakeDamage += HandlePlayerTakeDamage;
    }

    void InitiateCollectibles()
    {
        _collectibles = new List<GameObject>();
        var positions = new List<Vector3> {
            new Vector3(-28.5f, 13f, 0), // -> secret
            new Vector3(-27.5f, 13f, 0), // -> secret
            new Vector3(-26.5f, 13f, 0), // -> secret
            new Vector3(-9.5f, 5f, 0),
            new Vector3(22f, 5f, 0),
            new Vector3(62f, 5f, 0),
            new Vector3(12f, -2f, 0), // -> secret
            new Vector3(103f, 0, 0),
            new Vector3(113f, 1f, 0),
            new Vector3(133f, 5f, 0),
            new Vector3(137f, 12f, 0),
            new Vector3(123.5f, 5f, 0),
            new Vector3(126.5f, -5f, 0), // -> secret
            new Vector3(146.5f, 8f, 0), // -> secret
            new Vector3(147.5f, -2f, 0), // -> secret
            new Vector3(190f, 12f, 0)
        };
        foreach (var position in positions)
        {
            var collectible = Instantiate<GameObject>(_collectiblePrefab);
            collectible.transform.SetParent(_collectibleParent);
            collectible.transform.localPosition = position;
            collectible.GetComponent<Collectible>().OnAddPoints += HandleAddPoints;
            collectible.GetComponent<Collectible>().OnDestroyCollectible += HandleDestroyCollectible;
            _collectibles.Add(collectible);
        }
    }

    void InitiateLife()
    {
        _lifeGO = Instantiate<GameObject>(_lifePrefab);
        _lifeGO.transform.SetParent(_lifeParent);
        _lifeGO.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(50, -40, 0);
        _lifeGO.transform.localScale = new Vector3(1, 1, 1);
        _lifeGO.GetComponent<Life>().Instantiate(_life);
    }

    void InitiateScore()
    {
        _scoreGO = Instantiate<GameObject>(_scorePrefab);
        _scoreGO.transform.SetParent(_scoreParent);
        _scoreGO.transform.localScale = new Vector3(1, 1, 1);
        _scoreGO.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(275, -75, 0);
    }

    public void HandleAddPoints(int points)
    {
        _scoreGO.GetComponent<Score>().UpdateScore(points);
    }

    public void HandleDestroyCollectible(Collectible collectible)
    {
        collectible.OnAddPoints -= HandleAddPoints;
        collectible.OnDestroyCollectible -= HandleDestroyCollectible;
    }

    public void HandlePlayerTakeDamage(int hit)
    {
        var life = _lifeGO.GetComponent<Life>();
        life.LooseHeart(hit);
        if (life.IsAlive())
        {
            _playerGO.GetComponent<Player>().OnTakeDamage -= HandlePlayerTakeDamage;
            _playerGO.GetComponent<Player>().Killed();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_pauseUI.activeSelf)
            {
                _pauseUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                _pauseUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
