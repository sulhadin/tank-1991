
namespace WorldOfTanks.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Speech.Recognition;
    using System.Windows.Forms;
    public class SpeecRecognition
    {
        SpeechRecognitionEngine _srengine = null;
        public Action<CharacterGesture> OnAction;

        public SpeecRecognition()
        {
            try
            {
                _srengine = CreateSpeechEngine("en-US");
                _srengine.SpeechRecognized += SpeechRecognized;
                LoadGrammar();
                _srengine.SetInputToDefaultAudioDevice();
                _srengine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ses tanımlanması başarısız oldu");
            }
        }
        private SpeechRecognitionEngine CreateSpeechEngine(string culture)
        {
            foreach (var config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() != culture) continue;
                _srengine = new SpeechRecognitionEngine(config);
                break;
            }

            if (_srengine == null)
            {
                MessageBox.Show(culture + " bulunamadı.");
                _srengine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }

            return _srengine;
        }

        readonly List<KeyValuePair<string, CharacterGesture>> _pair = new List<KeyValuePair<string, CharacterGesture>>
            {
                new KeyValuePair<string, CharacterGesture>("sağ", CharacterGesture.Right),
                new KeyValuePair<string, CharacterGesture>("sol", CharacterGesture.Left),
                new KeyValuePair<string, CharacterGesture>("yürü", CharacterGesture.Move),
                new KeyValuePair<string, CharacterGesture>("dur", CharacterGesture.Stop),
                new KeyValuePair<string, CharacterGesture>("hadi", CharacterGesture.Fire),
                new KeyValuePair<string, CharacterGesture>("hoop", CharacterGesture.Stop),
                new KeyValuePair<string, CharacterGesture>("deh", CharacterGesture.Move),
                new KeyValuePair<string, CharacterGesture>("aşağı", CharacterGesture.Down),
                new KeyValuePair<string, CharacterGesture>("in", CharacterGesture.Down),
                new KeyValuePair<string, CharacterGesture>("üst", CharacterGesture.Up),

            };
        private void LoadGrammar()
        {
            var list = new List<string>();
            _pair.ForEach((d) =>
            {
                //if (list.All(x => x != d.Key))
                list.Add(d.Key);
                //if (list.All(x => x != d.Value.ToString()))
                //    list.Add(d.Value.ToString());
            });



            var wordsList = new Grammar(new GrammarBuilder(new Choices(list.ToArray())));
            _srengine.LoadGrammar(wordsList);


        }
        void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!(e.Result.Confidence > 0.6)) return;

            if (_pair.Any(d => d.Key == e.Result.Text))
                OnAction(_pair.FirstOrDefault(d => d.Key == e.Result.Text).Value);
        }




    }
}

