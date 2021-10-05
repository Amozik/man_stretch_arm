using System;
using System.Collections.Generic;
using System.Linq;
using ManStretchArm.Code.Interfaces;
using ManStretchArm.Code.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ManStretchArm.Code.Controllers
{
    public class CoinsController : ICleanup
    {
        private List<CoinView> _coins;

        public CoinsController()
        {
            _coins = Object.FindObjectsOfType<CoinView>().ToList();
            
            foreach (var coinView in _coins)
            {
                coinView.LevelObjectContactEvent += OnCoinContact;
            }
        }

        private void OnCoinContact(CoinView coin)
        {
            coin.LevelObjectContactEvent -= OnCoinContact;
            _coins.Remove(coin);
            GameObject.Destroy(coin.gameObject);
        }

        public void Cleanup()
        {
            foreach (var coinView in _coins)
            {
                coinView.LevelObjectContactEvent -= OnCoinContact;
            }
        }
    }
}