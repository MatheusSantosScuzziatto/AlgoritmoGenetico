using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoritmoGenetico
{
    public partial class Form1 : Form
    {
        private int numeroGeracoes = 0;
        private int geracoesMutadas = 0;
        private static Random sorteador = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void bt_iniciar_Click(object sender, EventArgs e)
        {
            Boolean prossegue;
            if (rb_geracao.Checked)
            {
                prossegue = verificaObrigatorios(new Object[] { tb_a, tb_b, tb_c, tb_a1initpop, tb_a2initpop, tb_a3initpop, tb_a4initpop, tb_numgeracoes });
                if(prossegue) 
                {
                    numeroGeracoes = int.Parse(tb_numgeracoes.Text);
                }
            }
            else
            {
                prossegue = verificaObrigatorios(new Object[] { tb_a, tb_b, tb_c, tb_a1initpop, tb_a2initpop, tb_a3initpop, tb_a4initpop });
            }
            if (prossegue)
            {
                geracoesMutadas = 0;
                lb_geracaobin.Text = "GERAÇÃO 1 (BINÁRIO)";
                lb_geracaodec.Text = "GERAÇÃO 1 (DECIMAL)";
                tb_historicobin.Clear();
                tb_historicodec.Clear();
                habilitarConfiguracao(false);
                mutacao(tb_a1initpop.Text, tb_a2initpop.Text, tb_a3initpop.Text, tb_a4initpop.Text);
            }
        }

        private void bt_proximageracao_Click(object sender, EventArgs e)
        {
            mutacao(tb_a1nextgen.Text, tb_a2nextgen.Text, tb_a3nextgen.Text, tb_a4nextgen.Text);
        }

        private void bt_gerartodas_Click(object sender, EventArgs e)
        {
            bt_proximageracao.Enabled = false;
            bt_gerartodas.Enabled = false;
            while (panelgeracoes.Enabled)
            {
                bt_proximageracao_Click(sender, e);
            }
            bt_proximageracao.Enabled = true;
            bt_gerartodas.Enabled = true;
        }

        private void rb_geracao_CheckedChanged(object sender, EventArgs e)
        {
            tb_numgeracoes.Enabled = true;
        }

        private void rb_max_CheckedChanged(object sender, EventArgs e)
        {
            tb_numgeracoes.Enabled = false;
        }

        private Boolean verificaObrigatorios(object[] camposObrigatorios)
        {
            for (int i = 0; i < camposObrigatorios.Length; i++)
            {
                if (((TextBox)camposObrigatorios[i]).Text.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("É necessário informar o campo " + ((Control)camposObrigatorios[i]).AccessibleName);
                    ((Control)camposObrigatorios[i]).Focus();
                    return false;
                }
            }
            return true;
        }

        private void habilitarConfiguracao(Boolean habilitado)
        {
            panelconfig.Enabled = habilitado;
            panelgeracoes.Enabled = !habilitado;
            if (habilitado)
            {
                lb_equacao.Text = "Ax²+Bx+C";
            }
            else
            {
                lb_equacao.Text = tb_a.Text + "x²+" + tb_b.Text + "x+" + tb_c.Text;
            }
        }

        private Boolean verificaLimite() //return true: o limite foi atingido, return false: o limite NÃO foi atingido.
        {
            geracoesMutadas++;
            lb_geracaobin.Text = "GERAÇÃO " + geracoesMutadas + " (BINÁRIO)";
            lb_geracaodec.Text = "GERAÇÃO " + geracoesMutadas + " (DECIMAL)";

            Boolean retorno;
            if (numeroGeracoes > 0)
            {
                retorno = geracoesMutadas == numeroGeracoes;
            }
            else
            {
                retorno = tb_a1nextgen.Text == "63"
                    && tb_a2nextgen.Text == "63"
                    && tb_a3nextgen.Text == "63"
                    && tb_a4nextgen.Text == "63";
            }
            if (retorno)
            {
                MessageBox.Show("Limite das gerações alcançado");
                habilitarConfiguracao(true);
            }
            return retorno;
        }

        private void mutacao(String a1, String a2, String a3, String a4)
        {
            tb_a1.Text = a1;
            tb_a2.Text = a2;
            tb_a3.Text = a3;
            tb_a4.Text = a4;

            #region Cálculos
            tb_a1dec.Text = Convert.ToInt32(a1, 2).ToString();
            tb_a2dec.Text = Convert.ToInt32(a2, 2).ToString();
            tb_a3dec.Text = Convert.ToInt32(a3, 2).ToString();
            tb_a4dec.Text = Convert.ToInt32(a4, 2).ToString();
            int a1dec = Convert.ToInt32(tb_a1dec.Text);
            int a2dec = Convert.ToInt32(tb_a2dec.Text);
            int a3dec = Convert.ToInt32(tb_a3dec.Text);
            int a4dec = Convert.ToInt32(tb_a4dec.Text);
            
            int a = Convert.ToInt32(tb_a.Text);
            int b = Convert.ToInt32(tb_b.Text);
            int c = Convert.ToInt32(tb_c.Text);

            int a1fx = ((a1dec*a1dec)*a + b*a1dec + c);
            int a2fx = ((a2dec*a2dec)*a + b*a2dec + c);
            int a3fx = ((a3dec*a3dec)*a + b*a3dec + c);
            int a4fx = ((a4dec*a4dec)*a + b*a4dec + c);
            tb_a1fx.Text = a1fx.ToString();
            tb_a2fx.Text = a2fx.ToString();
            tb_a3fx.Text = a3fx.ToString();
            tb_a4fx.Text = a4fx.ToString();

            int fxsum = a1fx + a2fx + a3fx + a4fx;
            
            double a1perc = (a1fx*100)/fxsum;
            double a2perc = (a2fx*100)/fxsum;
            double a3perc = (a3fx*100)/fxsum;
            double a4perc = (a4fx*100)/fxsum;
            tb_a1perc.Text = a1perc.ToString();
            tb_a2perc.Text = a2perc.ToString();
            tb_a3perc.Text = a3perc.ToString();
            tb_a4perc.Text = a4perc.ToString();
            #endregion

            #region Sorteio
            String a1sort = sorteio(a1perc, a2perc, a3perc, a4perc);
            String a2sort = sorteio(a1perc, a2perc, a3perc, a4perc);
            String a3sort = sorteio(a1perc, a2perc, a3perc, a4perc);
            String a4sort = sorteio(a1perc, a2perc, a3perc, a4perc);
            tb_a1sorteio.Text = a1sort;
            tb_a2sorteio.Text = a2sort;
            tb_a3sorteio.Text = a3sort;
            tb_a4sorteio.Text = a4sort;
            #endregion

            #region Crossover
            int linhaDeCorte = sorteador.Next(1, 5);
            String a1cross = a1sort.Substring(0, linhaDeCorte) + a2sort.Substring(linhaDeCorte); //recebe início de a1 e fim de a2
            String a2cross = a2sort.Substring(0, linhaDeCorte) + a1sort.Substring(linhaDeCorte); //recebe início de a2 e fim de a1

            linhaDeCorte = sorteador.Next(1, 5);
            String a3cross = a3sort.Substring(0, linhaDeCorte) + a4sort.Substring(linhaDeCorte); //recebe início de a3 e fim de a4
            String a4cross = a4sort.Substring(0, linhaDeCorte) + a3sort.Substring(linhaDeCorte); //recebe início de a4 e fim de a3

            tb_a1cross.Text = a1cross;
            tb_a2cross.Text = a2cross;
            tb_a3cross.Text = a3cross;
            tb_a4cross.Text = a4cross;
            #endregion

            #region Mutação
            String a1nextgen = mutacao(a1cross);
            String a2nextgen = mutacao(a2cross);
            String a3nextgen = mutacao(a3cross);
            String a4nextgen = mutacao(a4cross);
            tb_a1nextgen.Text = a1nextgen;
            tb_a2nextgen.Text = a2nextgen;
            tb_a3nextgen.Text = a3nextgen;
            tb_a4nextgen.Text = a4nextgen;
            #endregion

            plotarHistorico();

            #region Verifica se o limite foi atingido
            if (verificaLimite())
            {
                clear(new Object[]{ tb_a1fx, tb_a1perc, tb_a1sorteio, tb_a1cross, tb_a1nextgen, tb_a2fx, 
                    tb_a2perc, tb_a2sorteio, tb_a2cross, tb_a2nextgen, tb_a3fx, tb_a3perc, tb_a3sorteio, 
                    tb_a3cross, tb_a3nextgen, tb_a4fx, tb_a4perc, tb_a4sorteio, tb_a4cross, tb_a4nextgen });
            }
            #endregion
        }

        private void clear(object[] campos)
        {
            for (int i = 0; i < campos.Length; i++)
            {
                ((TextBox)campos[i]).Clear();
            }
        }

        private String sorteio(double a1perc, double a2perc, double a3perc, double a4perc)
        {
            double percent = sorteador.Next(0, 100);
            if (percent <= a1perc)
            {
                return tb_a1.Text;
            }
            else if (percent <= a1perc + a2perc)
            {
                return tb_a2.Text;
            }
            else if (percent <= a1perc + a2perc + a3perc)
            {
                return tb_a3.Text;
            }
            else
            {
                return tb_a4.Text;
            }
            return null;
        }

        private String mutacao(String aXcross)
        {
            //Cada bit tem 5% de chance de ser mutado. Portanto a cadeia de 6 bits tem 30% de chance de ser mutada
            int mutacao = sorteador.Next(0, 100);
            if (mutacao <= 30)
            {
                int bitMutacao = sorteador.Next(0, 120);
                if (bitMutacao <= 20) //mutar sexto bit
                {
                    aXcross = mutarBit(aXcross, 5);
                }
                else if (bitMutacao <= 40) //mutar quinto bit
                {
                    aXcross = mutarBit(aXcross, 4);
                }
                else if (bitMutacao <= 60) //mutar quarto bit
                {
                    aXcross = mutarBit(aXcross, 3);
                }
                else if (bitMutacao <= 80) //mutar terceiro bit
                {
                    aXcross = mutarBit(aXcross, 2);
                }
                else if (bitMutacao <= 100) //mutar segundo bit
                {
                    aXcross = mutarBit(aXcross, 1);
                }
                else //mutar primeiro bit
                {
                    aXcross = mutarBit(aXcross, 0);
                }
            }
            return aXcross;
        }

        private String mutarBit(String aXcross, int pos)
        {
            char[] bitArray = aXcross.ToCharArray();
            if (bitArray[pos].Equals('1'))
            {
                bitArray[pos] = '0';
            }
            else
            {
                bitArray[pos] = '1';
            }

            String retorno = "";
            foreach(char bit in bitArray) {
                retorno += bit.ToString();
            }
            return retorno;
        }

        private void plotarHistorico() {
            tb_historicobin.AppendText(tb_a1.Text + " - " + tb_a2.Text + " - " + tb_a3.Text + " - " + tb_a4.Text + Environment.NewLine);
            tb_historicodec.AppendText(tb_a1dec.Text + " - " + tb_a2dec.Text + " - " + tb_a3dec.Text + " - " + tb_a4dec.Text + Environment.NewLine);
            
        }
    }
}
