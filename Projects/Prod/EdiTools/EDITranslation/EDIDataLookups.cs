using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace EDITranslation
{
    public static class EDIDataLookups
    {
        public static string EnercrossPrivateKey= @"-----BEGIN PGP PRIVATE KEY BLOCK-----
                                               Version: GnuPG v2.0.22 (MingW32)
                                               
                                               lQO+BFJ84NoBCADl+twTnGwNHWOryNgDqD+FshfsIYsLRIa7yb0j1IbttG3tyE1O
                                               iSo2inf0Q73j45JvA5ndxw8zPGdRpNE3wfXNTC+3fDbWhFzFpbxz5yuIp43yw0J5
                                               WWZeLditMHiqw7No38o1O9Xfenw40vy7qzK/cE93qFzO4ijukqUiK+T2UIXShWX5
                                               /SxpKAcItFzqSxUMCUMnvMrG04qpG4EtzekcBQgpnwp2NqUlYVtCRqmaRHrD/dTC
                                               PQjSHFoHocL8pMAe7DMo7FDuYlge3U5+eefwZHYFchy21B2agqbTWmdXExQm6q0Z
                                               RPinR+hUUvuGF0cOi14BKGOBNfB8YIioK8G/ABEBAAH+AwMCd1f7XGqW+061jhJx
                                               G7m31IeqMMacHB1QeQmC0KH6s7J4wc8Dx+Q4lO/KMaw1hih5Tk/jcDSowwnWJYXw
                                               rg2ZoW4AojFDp5GSuYXYZu8ZPU8YSOHycUU9HxKiAa/m3D7eXEcwLSWano2PTi3F
                                               D6IdCnKHHB+MTcfOGmSbl/cDc3/6mCb4X9Ntlu/prR9i1GR8oXWvOGjza9v9gOdo
                                               LlemmLpCdV7EqKzAHmBFHyHectEcU6T995BJMCBdgnopHvKJO5TulVk670pLoC6r
                                               BuBPuira9aVo/0Q8P4ZBLV6Oo1ytfJZiAtn109ij+KmQJA+lZNUt6MRVWsq6m72W
                                               eS9Gjqfwx5H4x31zAdfs3JXN2fZ4UQkORsJl0VhlsC5KTsjgnr+DL67MxOq1nyQT
                                               /S5xL18NnKz9x5qFs9UMC3b9uh0rCsOxNHJPZG7XO41MuzCa/nVS2823199y+uc4
                                               /idd/mgjEID+YXcYOPlsmTofzxFLsRFtf2Ww+/tjDoV9e72x+dVUNjdzoYqkBuUU
                                               KLnj4VouaNkFXJHw2rLSgF26CHfjtY0uYwXrkY8mUL9/3Pit/+P+WrXt6WFVRZWR
                                               6cokGIX1lOpsEPjOpopNrVn33dAai0eQar1O008yAxlgrwDfBq9zN2yVfAlIFLxk
                                               NUBGUreYW70u3JZ4gV/KZDMgr9Iu69TZ3d9IBm+qNJtveQVVzMJmMicRXbez8KVm
                                               yB4qp+ClAWcSQ3GUSLOBgtncOGb40rC035s2/8+2dSj28RBnIXdrdtJjvgj1VcX/
                                               Uwpzq6ab8iOSvXdyjhezlxb6UyddkKOtIxHfr2NHBhqWObGdEg4ThK5loe1dxUUI
                                               RXBtSzzhtdc/Z5XeKGpfTZvhhhaoAl6QMB2pjfTRkjYHr3I2qoDwx1uV87NNBbpB
                                               I7QpS2V0YW4gU2hhcm1hIDxrZXRhbi5zaGFybWFAZW5lcmNyb3NzLmNvbT6JATkE
                                               EwECACMFAlJ84NoCGw8HCwkIBwMCAQYVCAIJCgsEFgIDAQIeAQIXgAAKCRAmdL7s
                                               BsBiGuGRB/4o566P3lmjS3cDhXftB9TEmvseRLJEGtAc4NBKpTjuHQm53wKDOlJM
                                               OXpE6xtVXbAHxlIPhGvSqPWnui3CBmdulvjIrk6EBcEd2TualP5v++oyXAF2kOAZ
                                               qmyvDFqskdelKRRo7Je0DYzQYRKJLLlE7TS44kbbjAq8WVVKr0peCmZRA6PS7jDG
                                               LO79tbn9dYSenyD5esTrKmyiX56NM4qUu1uSgLYBbObneHuPhe9en2zUUz6l/1k0
                                               pvlWfaXYSCYd239v603MIqBxh0Exz/jNWuVu2j1SvZxxG4P0u1BHWiy9pPM74p2m
                                               3JQ6S0AkvliXskVNMDr3z5wuz7G7+lQH
                                               =vnwk
                                               -----END PGP PRIVATE KEY BLOCK-----
                                               ";
        public static string EmeraPrivateKey = @"-----BEGIN PGP PRIVATE KEY BLOCK-----

                                                   lQdGBFrLBAUBEADnmA9pD90FMfw0MMcuzdoIArNp1oDFXMS2FEqVuLzPTggX7QWZ
                                                   jqOxuu2vOzmJRJYgdXV8h1r+a8jihK/qit0ObgD/xlD4GVg8WorJlpy/QRogdizn
                                                   y7EAZQAupaQjSMHjLPfDIGiL7PNtf4yy2od6T56944v9ckYUeLYi28QsmpkVP0MU
                                                   LGzwlJgI4zICxyUMdTyckw99nG1yhgaE2QPtB7ZKnwg0Rtxh/sTUyGmyUzEmdg4q
                                                   E1ZWo6N5G0IUf3cMVL3ogRm5PXfIWNVdCnsWVkqhQxpJ4/2Ea/JGUEsOpbTdW5xu
                                                   tNx4e9cq+RvjjdcqpJgjWyUNYOwE9Sx34wXgLJljnhCIHGdfnmLjuY0a1u9nONks
                                                   aKpLMLLAjGvHnJ+z893StXzpVwaJFWFgG4bHEqCf8dory+2PQj5UxvOBVksFpnUQ
                                                   JY0Zogw8BSdSPFQZjn8zYNfP3bhkk7+9OvVoJU+r6j2A2jSePtfSJQDBujA9yRW7
                                                   D33pQH0VIf7W7sKkJ3T1rs6Ndbe0oneY4xVCQG1xFCelhc9RN6YBEN2mZgDWhRtG
                                                   2p9tgeSdOlmKmBwlkMQdtkrE4u2pJuAAaWI93kvm5kjONRDAW0N0sd1FfRhHYv5L
                                                   b46QWKTJXQtsS5xzWOTq+nDWArVdAxaabisN967I2V+90pnZ9Uk6pmnajwARAQAB
                                                   /gcDAqU9nqe3HeKpvqBCmnGcJvAA9CZWZ9bHMpFxwu3RXuMTHDrXapVAhL8isTYo
                                                   yAbGW/cIVI1UZ43nxxl6DTmqDSwW2FXEZyH9U4IWM+ypK+MY5Adn2TK3//Laui2D
                                                   8q0qhvsXCgN4xT7JuRvywOGa8ilblRwUQxzRNr1yaeSw6UyAVJFE7+WbNvbD8Mbn
                                                   WWWHClfxkr8as1nRKZl/3kHhqaqAOdj9Sc2+KxCPdvmkGbK3fVhlxpnaGaVFoGRR
                                                   +stARbKG+vO+8/GO7IgyfEuSCxWe4ezxo+rwhhYmzpLM84sLJGshKi5wDSa+EVnu
                                                   6rPZlf8t6oO5dT8PFIPcxk8vyD5HWdg/QC23i2XExImw1HoC3pdZ3oWjEh40A7nb
                                                   gjrPsk/STtFjC6yqJT8+5wqz87X/9dvpKd4Ad5lDjNedVWChMDt1XiS3j6SbDrxt
                                                   ngjE48lYwzp9ep+9NKvinMUMSyD4ftSqignJ0pkhpwHstm92GnSehF95cm6B/Ld+
                                                   75iZfd/NXTtTGbEMNukcgzJVitUMD1Ox0lF7stWkwF1jkfL2qy+NWeV2ovTv7CRu
                                                   NhP+mxUgx3jrMfWq1pMe9+5kY8NHoc6gN3iouO0MnyLM2gAnonUmUqTTh1kO1++s
                                                   xOYWE80iR/ExGZ73jdXrEKr6xnbGHCOdj2v3tdldnMfif6oUtg1E2ZVu7umf2RGf
                                                   k//F4TTQycfkKkN87oQAwv2nOA3TcI+sjCFBP9eMG92DB5xC1KKT4jlLcnExq/tN
                                                   zAIvC+9NscxYUVir+aDtRFG6t1+fdFiGfXNPgIBBzKfwb3DTkY17tGROv0gCHXYw
                                                   s14BinOXl6KnSO0NOFEZDE2d0zMKcOV24XcyU23F/P+IiSQrEWpb11ctmXZ6B6gY
                                                   w92Rn/n9afs/lUcmtvr4uUsOzrMniS61h9iWMh1BLM0NizJ+bVwXubQig0sKQ33M
                                                   C/VxyXAAvoB6/f0UHI+rrZIHiK4wAbQPNghl2B/CtqVEnMVDn4yjQDfpcXvn13T3
                                                   KgwgVzVyMLXQaHmaHKb4CUDSLREoB1IYTwJDXaQ0HTTOC441/H2ce4mGRYvAl44C
                                                   tIN+lOeHcQmL/8xnqBtxg95A4BsmLH4bmDPeiRDXmbzBqvzekm/YSbfj1kGUx1Pr
                                                   QPji+au1XVxXNV/GPapiNNuzjI2vVjdlsyJ46j2h8Mim+FSScV1dbqx7gNKhq8KM
                                                   NxaQflqqy/bUB+a96gXYp5l8ehBU/Z8oiidn6C6whoFH+0rvK8m2AF9ysEax8Zxk
                                                   c7HUi3bbTU1DyV7DdCgioddeQaloFLIIRFIPUfCFfIPEBmFaYuL7EZyqKpYwKGWU
                                                   S2MvGES1bVoQlmka5wWAOYDDxx8S8Wll7jEVIdtRADHsh+s6ZiDWM8GpCQ8udnqJ
                                                   ILZF1oYtsjtiaMhaIq+4WDbjoiGT3BgoeW06VF1zFFtg50TJIGQKQ/1KhnjfFAtE
                                                   XWAEUXY67aDpokT2yutEiYp1mu0qJTrwPT9eXL7vGiA1E6lK1EmDjxD0/+zyKFTd
                                                   049vULp5RL8uHCRxonmAWD2vRXbJ3LDgNTTwZ5buw1UbZfj2tUZ05yLZV0mNmUyq
                                                   Rf6AZCjFIjJyWxxmyI1kG3WCB8aeLbIM+nKaN4Ibh82CzcgWZUL1A9GBMZbuykCe
                                                   R+9MHVOdUwSKHWRTItSqjxPH/VlycpnM53GRcwn6OBOhB3IQS9Q9Vk/u+erUMB/m
                                                   ULb9NUJ1e//ykJglNm4O8yb2EGXJ5oY6RHa4VdgSmjdT7A4EHciMHJC0BUVtZXJh
                                                   iQJOBBMBCAA4FiEEWRiTg2SiA/pyioV2Mu7fL53QbKEFAlrLBAUCGw8FCwkIBwIG
                                                   FQoJCAsCBBYCAwECHgECF4AACgkQMu7fL53QbKFmQg//fDg9V7DWF5Q3SjmiIKqV
                                                   ZKHpgfbMMzX2JcDsstcBQzPFOsQxj4c0BOiUoUAeSxYwDt9xHHoUeVggZ6lnuavr
                                                   W2fRB74neIvdvE+FeVNEee77NHeBKDe5U839HqsCgfVO5K90EvyEMd4FhQYNPYJW
                                                   8pP1AJo6OIKlpOvugaoFeoVNJeT5U3V5R70gEwwJXZ7BIJfnS05bXHGc/YZz9VjY
                                                   IgAMKT5rZldii70sSik34wS2GxhGS9aqKH9JQXQEUiXqlKW0cZTpl8dgKLGvhvBz
                                                   /uE4DFGpjB5N5gMJeyIVTIaCxi7yW80SvFG/ciJnpFqi1R7meKaishbWu03qOPgz
                                                   PEia/l+H7wuVXNh3hSD6Bd1ymo6a7ShjUKpxoiPH2DDM5w3XzABj+65TWkvLxnt2
                                                   ahsw7rVDUbP0eH1sjFPxHs3S1N0QP8VOPW1FReGPYmN+cAx0uSSZF2Q1OpB0kpRA
                                                   61rkkzzICSOG+SBkIfk0aU+f1XkGjunkKS9EBlAZhYckFvhCiG3pDlufNf6+aIdR
                                                   gZvFLekLFgsIbp6uwBmvjVO7OJCLzbPoch7NRF/4cd6bf+Rv0K2IKlnhQTHdIWpy
                                                   8No28i8FyjPTWiJkozXOak3A3laGjmJGfYkXoXO05bsiRm/rRSkYGut0U/wb+DTC
                                                   jmHjjly5xSiyfrkHVRNmgX0 =
                                                   = ZVG1
                                                   ---- - END PGP PRIVATE KEY BLOCK-----
                                                   ";
        public static string EnercrossPassphrase = "HNaci6ux";
        public static string EmeraPassphrase = "7%lWpxKbOng9K(^Kb(e!(W8u";
        public static string EnercrossTestConnectionString = @"Data Source = trading.enercross.com; User Id = appEnercross; Password=enercross501;Initial Catalog = NomDb; Connection Timeout = 30000";
        public static string EnercrossProdConnectionString = @"Data Source = shell.nom1done.com; User Id = sa; Password=Wednesday1705;Initial Catalog = NomDb; Connection Timeout = 30000";

        public static List<string> SeparatorsLookup = new List<string>
        {
            "Algonquin,006951446,*,~", "ANR,006958581,|,~","Bobcat,614834559,*,~",
            "Cheyenne,143363542,*,~",  "CIG,006914865,*,~", "COL GAS,054748041,*,~",
            "COL GULF,007854581,*,~",  "Crossroads,808593164,*,~", "Dominion,116025180,*,~",
            "Egan,835460478,*,~",  "EPNG,008001703,*,~",  "FGT,006924518,*,~",
            "GLGT,046077343,*,~",          "Gulf Crossing,828918214,*,~",           "Gulf South,078444247,,",
            "Iroquois,603955949,*,~",            "MEP,808150895,*,~",            "Millennium,607821050,*,~",
            "Mojave,620365619,*,~",            "NGPL,006931794,|,~",            "NNG,784158214,~,",
            "Panhandle,045256641,*,~",            "REX,784256161,*,~",            "Ruby,013541571,*,~",
            "SNG,006900518,|,~",            "Tetco,007932908,*,~",            "Texas GAS,115972101,~,",
            "TGP,001939164,|,~",            "TIGT,828257431,*,~",            "TPC,013665749,*,~",
            "Transco,007933021,~,",            "Trunkline,007933062,*,~",            "WIC,030669048,*,~"
        };

            

        public static Hashtable PipelineEncryptionPublicKeys = new Hashtable()
        {
            //ANR Public Key
            { "006958581","-----BEGIN PGP PUBLIC KEY BLOCK-----"+
                    "Version: McAfee E-Business Server v8.5.2 - Full License"+
                    "mQCNAzNnxj0AAAEEANHIg4iiWtT/Z4wr7B1xDm4lek/bRi/wRSypIBHis9mI2P26"+
                    "4ViWgckSEurFYtuUmxVO8onzBly+G/2KPq+iYU1msm3xy7+cJDtJDGSQJJT7ygnB"+
                    "f7upbNo2jgFYUJ1GI1VGWUtPC8Fe8CYbtHmcStZ0NlKH8AmHvgyd15ShM1CtAAUR"+
                    "tAVBTlJQTA=="+
                    "=WHeu"+
                    "-----END PGP PUBLIC KEY BLOCK-----"
            },
            //TGP Public Key
            {
                "001939164",
                "-----BEGIN PGP PUBLIC KEY BLOCK-----"+
                "Version: PGPfreeware 6.5.3 for non-commercial use <http://www.pgp.com>"+

                "mQCNA0PhDoMAAAEEALHvmEwL9DakTmxsxxZdeWQTYpjQuvij5hyEIOPe9XRd6Avm"+
                "Xn4e9ZN32dxlJdLwNDYS09Yo1SaGHyL8L+Kn5Zp610knSq3A9+ps6260+0LRcbpS"+
                "ay47RQfMXWs3diEMVO2kwwF5qnGLzb5TALoWnDecS6WMZWwHamIonwwyzdafAAUR"+
                "tDFLSU5ERVIgTU9SR0FOIDIwMDYgPHN0YW5fdGhvbWFzQGtpbmRlcm1vcmdhbi5j"+
                "b20+iQCVAwUQQ+EOg2IonwwyzdafAQFQPwP+Kr7I9a/T0sewoyoIIe/mZ634uueu"+
                "xISwgzw2j38DqSQtSs5MHtwEfQO6dldLYgrqiXpYDsnhhD/t74k6cAmoJjvPvFNg"+
                "hnQyqctw9XMDCLN2epXKQJJebMXZuB0ksEJdr5ga9NbcaIba4KsjNFXinGr/wUvr"+
                "HcXhiHGAUXeevQ0="+
                "=l81f"+
                "-----END PGP PUBLIC KEY BLOCK-----"
            },
            //SNG Public Key
            {
                "006900518",
                "-----BEGIN PGP PUBLIC KEY BLOCK-----"+
                "Version: PGPfreeware 6.5.3 for non-commercial use <http://www.pgp.com>"+

                "mQCNA0PhDoMAAAEEALHvmEwL9DakTmxsxxZdeWQTYpjQuvij5hyEIOPe9XRd6Avm"+
                "Xn4e9ZN32dxlJdLwNDYS09Yo1SaGHyL8L+Kn5Zp610knSq3A9+ps6260+0LRcbpS"+
                "ay47RQfMXWs3diEMVO2kwwF5qnGLzb5TALoWnDecS6WMZWwHamIonwwyzdafAAUR"+
                "tDFLSU5ERVIgTU9SR0FOIDIwMDYgPHN0YW5fdGhvbWFzQGtpbmRlcm1vcmdhbi5j"+
                "b20+iQCVAwUQQ+EOg2IonwwyzdafAQFQPwP+Kr7I9a/T0sewoyoIIe/mZ634uueu"+
                "xISwgzw2j38DqSQtSs5MHtwEfQO6dldLYgrqiXpYDsnhhD/t74k6cAmoJjvPvFNg"+
                "hnQyqctw9XMDCLN2epXKQJJebMXZuB0ksEJdr5ga9NbcaIba4KsjNFXinGr/wUvr"+
                "HcXhiHGAUXeevQ0="+
                "=l81f"+
                "-----END PGP PUBLIC KEY BLOCK-----"
            },
            //NGPL Public Key
            {
                "006931794",
                "-----BEGIN PGP PUBLIC KEY BLOCK-----"+
                "Version: PGPfreeware 6.5.3 for non-commercial use <http://www.pgp.com>"+

                "mQCNA0PhDoMAAAEEALHvmEwL9DakTmxsxxZdeWQTYpjQuvij5hyEIOPe9XRd6Avm"+
                "Xn4e9ZN32dxlJdLwNDYS09Yo1SaGHyL8L+Kn5Zp610knSq3A9+ps6260+0LRcbpS"+
                "ay47RQfMXWs3diEMVO2kwwF5qnGLzb5TALoWnDecS6WMZWwHamIonwwyzdafAAUR"+
                "tDFLSU5ERVIgTU9SR0FOIDIwMDYgPHN0YW5fdGhvbWFzQGtpbmRlcm1vcmdhbi5j"+
                "b20+iQCVAwUQQ+EOg2IonwwyzdafAQFQPwP+Kr7I9a/T0sewoyoIIe/mZ634uueu"+
                "xISwgzw2j38DqSQtSs5MHtwEfQO6dldLYgrqiXpYDsnhhD/t74k6cAmoJjvPvFNg"+
                "hnQyqctw9XMDCLN2epXKQJJebMXZuB0ksEJdr5ga9NbcaIba4KsjNFXinGr/wUvr"+
                "HcXhiHGAUXeevQ0="+
                "=l81f"+
                "-----END PGP PUBLIC KEY BLOCK-----"
            },
            //Tetco Public Key
            {
                "007932908",
                "-----BEGIN PGP PUBLIC KEY BLOCK-----"+
                "Version: PGP 6.5.8"+

                "mQCNAzNf4+YAAAEEANU17kpbvM0Xz9AN69L3rd+2vYzgOSEexihSZM/4pWQdjpQF"+
                "VjEugqjyM1IziWL4qJngqTh6tY6Fy+xd2JPkawMdeba+NTxUfuPYuh830ZbMzb00"+
                "iipeErdyDqzGFtQimeiFX3XCoMY+9SSe6snRoMuIgmwmpfvaUvHCRYHRYEAjAAUR"+
                "tB5QYW5FbmVyZ3kgRURNIDEwMjQgQXByaWwgMjQgOTeJAJUDBRA92mkd8cJFgdFg"+
                "QCMBAVQzA/92YaCIffOJvmpS1d+2mkVqBT7UVESxqoVCDIngoq55CPHHyY7AFEVC"+
                "dSHOIHsvRZ2wiH9j2gxzt27hk7Zb17W6mF+HeAtgwBy7RHiAE77xePi4Zq8SSshQ"+
                "Gu3ZREigxv6JfpcUSGXYLRiU27YalyP7mbY86knd1K25HpTsyi4eKYkAlQMFED3a"+
                "atatDOXQmau97wEBLrIEAIalJ9pgbb5q9VHSbAIbAPIzu4UZ4q6R3BXlq8DDssJv"+
                "89A1Vd54gU7P7d3T19z0RsGXVWhUCg8vJo9RQJHD55jwfNq/BC/mDd2Cpf+x90ZU"+
                "IjYqZMucE5djnggjypdcmHDBSTxA8J3vBR4nZTpjzzCLwfIcRGhJEpMcILYWkt7a"+
                "=RpJm"+
                "-----END PGP PUBLIC KEY BLOCK-----"
            }
        };

       

       
    }
}
