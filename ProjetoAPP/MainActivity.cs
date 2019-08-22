using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Plugin.ExternalMaps;
using System;


namespace ProjetoAPP
{
    [Activity(Label = "MEU PET", Theme = "@style/Base.Theme.AppCompat.Light.DarkActionBar", MainLauncher = true, Icon ="@drawable/Miniatura")]
    public class MainActivity : AppCompatActivity
    {
        // Variáveis Globais
        public string[] partes;
        string[] words;
        public bool _cliente;
        public bool controller=false;
        public int found;
        public int ctrl;
        public double LAT;
        public double LNG;
        Button varOk;
        Button varPronto;
        Button varInfo;

        public string _url, _SEI, _html, _nomePet, _nascimentoPet,
        _racaPet, _nomeDono, _emailDono, _endereco, _telefone,_Lat,_Lng;
        ImageView varDados, varLocalizacao, varVoltar, varEnviar, varAtt, varVerifica, varAnuncio;
        
        
        //Inicialização
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Inicia a página principal...
            SetContentView(Resource.Layout.LoginScreen);

            // Declarações...
            varPronto = FindViewById<Button>(Resource.Id.loginButtonPronto);
            varInfo = FindViewById<Button>(Resource.Id.loginButton);

            // Ações...
            varPronto.Click += Pronto_click;
            varInfo.Click += delegate
            {
                Toast.MakeText(ApplicationContext, "     Funcionamento incorreto? jackson_silva31@hotmail.com ", ToastLength.Long).Show();
            };
        }
        // Métodos
        private void Pronto_click(object sender, EventArgs e)
        {
            // Atribui o valor do SEI digitado a uma variável interna e inicia a página principal.
            if (FindViewById<EditText>(Resource.Id.loginEntradaSenha).Text != "")
            {

                _SEI = "?SEI=" + FindViewById<EditText>(Resource.Id.loginEntradaSenha).Text;            
            }

            SetContentView(Resource.Layout.MainScreen);

            varDados = FindViewById<ImageView>(Resource.Id.IMG_Dados);
            varLocalizacao = FindViewById<ImageView>(Resource.Id.imgLocalizacao);
            varAnuncio = FindViewById<ImageView>(Resource.Id.imgAnuncio);

            varAnuncio.Click += Anuncio_click;
            varDados.Click += Dados_click;
            varLocalizacao.Click += Localizacao_click;

        }
        private void Anuncio_click(object sender, EventArgs e)
        {
            // Exibe a página de anúncios.
            SetContentView(Resource.Layout.Anuncios);

            varOk = FindViewById<Button>(Resource.Id.AnuncioButtonOk);

            varOk.Click += Voltar_click;
        }
        private void Localizacao_click(object sender, EventArgs e)
        {
            // Capitura as cordenadas e exibe o mapa
            try
            {
                ConnWithPHP enviando = new ConnWithPHP();

                _html = enviando.Conexao(_SEI, "");

                string text = _html;

                char[] delimiterChars = { '!', ',', ':' };

                words = text.Split(delimiterChars);
                _Lat = words[4];
                _Lng = words[6];

                LAT = Convert.ToDouble(_Lat) / 100000;
                LNG = Convert.ToDouble(_Lng) / 100000;


                CrossExternalMaps.Current.NavigateTo("", LAT, LNG);
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "Verifique a sua conexão.", ToastLength.Long).Show();
            }
        }
        private void Dados_click(object sender, EventArgs e)
        {
            // Realiza a conexão com o php e atribui os 'returns' a variáveis internas.
            try
            {
                ConnWithPHP enviando = new ConnWithPHP();

                _html = enviando.Conexao(_SEI, "");

                string text = _html;

                char[] delimiterChars = { '!', ',', ':' };

                words = text.Split(delimiterChars);

                _cliente = true;
                _nomePet = words[8];
                _nascimentoPet = words[14];
                _racaPet = words[10];
                _nomeDono = words[12];
                _emailDono = words[16];
                _endereco = words[20];
                _telefone = words[18];

                SetContentView(Resource.Layout.DataScreen);


                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varAtt = FindViewById<ImageView>(Resource.Id.imgAtt);
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "Verifique a sua conexão.", ToastLength.Long).Show();
            }

            if (_cliente == true)
            {
                FindViewById<TextView>(Resource.Id.edtDono).Text = _nomeDono;
                FindViewById<TextView>(Resource.Id.edtPetNome).Text = _nomePet;
                FindViewById<TextView>(Resource.Id.edtEmail).Text = _emailDono;
                FindViewById<TextView>(Resource.Id.edtEndereco).Text = _endereco;
                FindViewById<TextView>(Resource.Id.edtPetNascimento).Text = _nascimentoPet;
                FindViewById<TextView>(Resource.Id.edtPetRaca).Text = _racaPet;
                FindViewById<TextView>(Resource.Id.edtTel).Text = _telefone;

            }

            varVoltar.Click += Voltar_click;
            varAtt.Click += Atualizar_click;

        }
        private void Atualizar_click(object sender, EventArgs e)
        {
            // Habilita o modo de alteração nos campos da página de dados
            ImageView substituirIMG = (ImageView)FindViewById<ImageView>(Resource.Id.imgAtt);
            substituirIMG.SetImageResource(Resource.Drawable.IMG_Enviar);

            FindViewById<TextView>(Resource.Id.edtDono).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtEmail).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtEndereco).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtPetNascimento).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtPetNome).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtPetRaca).Enabled = true;
            FindViewById<TextView>(Resource.Id.edtTel).Enabled = true;

            varEnviar = FindViewById<ImageView>(Resource.Id.imgAtt);
            varEnviar.Click += Enviar_click;
        }
        private void Enviar_click(object sender, EventArgs e)
        {   // Envia as alterações feitas para o php caso não haja erros de preenchimento.
            controller = false;
            if (FindViewById<TextView>(Resource.Id.edtDono).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Verifique o nome do Dono!", ToastLength.Long).Show();
                controller = true;

                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (FindViewById<TextView>(Resource.Id.edtPetNome).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Verifique o nome do animal!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }   

            if (FindViewById<TextView>(Resource.Id.edtEmail).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Verifique seu email!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (FindViewById<TextView>(Resource.Id.edtEndereco).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Houve um Erro no envio!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (FindViewById<TextView>(Resource.Id.edtPetNascimento).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Houve um Erro no envio!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (FindViewById<TextView>(Resource.Id.edtPetRaca).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Houve um Erro no envio!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (FindViewById<TextView>(Resource.Id.edtTel).Text == "")
            {
                Toast.MakeText(ApplicationContext, "Houve um Erro no envio!", ToastLength.Long).Show();
                controller = true;
                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varVerifica = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varVerifica.Click += Enviar_click;
            }

            if (controller != true)
            {
               

                _url = "&Nome_animal=" + FindViewById<EditText>(Resource.Id.edtPetNome).Text;
                _url = _url + "&Nascimento=" + FindViewById<EditText>(Resource.Id.edtPetNascimento).Text;
                _url = _url + "&Raca=" + FindViewById<EditText>(Resource.Id.edtPetRaca).Text;
                _url = _url + "&Endereco=" + FindViewById<EditText>(Resource.Id.edtEndereco).Text;
                _url = _url + "&Nome_dono=" + FindViewById<EditText>(Resource.Id.edtDono).Text;
                _url = _url + "&Email=" + FindViewById<EditText>(Resource.Id.edtEmail).Text;
                _url = _url + "&Contato=" + FindViewById<EditText>(Resource.Id.edtTel).Text;               

                ConnWithPHP enviando = new ConnWithPHP();

                _html = enviando.Conexao(_SEI, _url);

                _cliente = true;

                FindViewById<TextView>(Resource.Id.edtDono).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtEmail).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtEndereco).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtPetNascimento).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtPetNome).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtPetRaca).Enabled = false;
                FindViewById<TextView>(Resource.Id.edtTel).Enabled = false;

                ImageView substituirIMG = (ImageView)FindViewById<ImageView>(Resource.Id.imgAtt);
                substituirIMG.SetImageResource(Resource.Drawable.IMG_Atualizar);

                varVoltar = FindViewById<ImageView>(Resource.Id.imgVoltar);
                varAtt = FindViewById<ImageView>(Resource.Id.imgAtt);

                varVoltar.Click += Voltar_click;
                varAtt.Click += Atualizar_click;

            }
        }
        private void Voltar_click(object sender, EventArgs e)
        {
            // Volta à página principal 
            controller = false;
            SetContentView(Resource.Layout.MainScreen);

            varDados = FindViewById<ImageView>(Resource.Id.IMG_Dados);
            varLocalizacao = FindViewById<ImageView>(Resource.Id.imgLocalizacao);
            varAnuncio = FindViewById<ImageView>(Resource.Id.imgAnuncio);

            varAnuncio.Click += Anuncio_click;
            varDados.Click += Dados_click;
            varLocalizacao.Click += Localizacao_click;

        }
    }
}

