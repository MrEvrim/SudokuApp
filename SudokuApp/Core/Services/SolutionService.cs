using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SudokuApp.Core.Contexes;
using SudokuApp.Core.Enums;
using SudokuApp.Core.Helpers;
using SudokuApp.Core.Models;
using System.Reflection;

namespace SudokuApp.Core.Services;

public class SolutionService
{
    private readonly SudokuContext _dbContext;

    public SolutionService(SudokuContext dbContext)
    {
        _dbContext = dbContext;
    }


    public SolutionHead FindSolution(out ResultObject result, uint tryCount,uint totelCount)
    {
        string opc = $"{MethodBase.GetCurrentMethod().DeclaringType.FullName}.{MethodBase.GetCurrentMethod().Name}";
        string title = "Çözüm Bul";

        try
        {
            DateTime startTime = DateTime.Now;

            //boş bir veri listesi oluştur
            List<SolutionItem> solutionItems = GetEmptyItemList();
            //verileri doldur
            uint totelCounter = 0; // veri çekme olayı out ile dışarı aldık.
            SolutionStatuses solutionStatuses = FillItems(out totelCounter,solutionItems,1000,100000000);

            //veri listesi değerlerini ard arda ekle ve key olarak string hale geittr
            string key = string.Empty;
            foreach (SolutionItem item in solutionItems)
            {
                key += item.Value.ToString();
            }
            //sonuç uygun gekdi ise veri listesi doğrula
            string chechResponse = string.Empty;
            if (status == SolutionStatuses.Succesful)
                status = CheckSolution(out chechResponse, solutionItems);

            TimeSpan ts = DateTime.Now - startTime;

            SolutionHead head = new()
            {
                Id = 0,
                StartTime = startTime,
                TotalTime = ts.TotalSeconds,
                FinishTotalCount = totelCounter,
                LoopTotalCount = totelCounter,
                Description = chechResponse,
                Key = key,
                Status = status,
                StatusDescription = string.Empty,
                SolutionItems = null,

            };


            result = SystemMessageManager.MethodSuccessResultObject(title, opc);
            return head;
        }
        catch (Exception ex)
        {
            result = SystemMessageManager.MethodExepcionResultObject(title, opc, ex);
            return null;
        }
    }
    private List<SolutionItem> GetEmptyItemList()
    {
        List<SolutionItem> solutionItems = new();

        // tüm hücreler için değerleri 0 olan, adresleri dolu bir liste oluştur
        byte cellNum = 1;
        for (byte row = 1; row < 10; row++)
        {
            for (byte col = 1; col < 10; col++)
            {
                solutionItems.Add(new SolutionItem
                {
                    Id = 0,
                    SolutionHeadId = 0,
                    CellNumber = cellNum++,
                    RowNumber = row,
                    ColumnNumber = col,
                    Value = 0,
                    GroupNumber = FindGroupNumber(row, col)
                });
            }
        }

        return solutionItems;
    }
    private byte FindGroupNumber(byte row, byte col)
    {
        // satır ve sütun numaralı bazında grup numarasını bul
        byte group = 0;

        if (row >= 1 && row <= 3 && col >= 1 && col <= 3) group = 1;
        else if (row >= 1 && row <= 3 && col >= 4 && col <= 6) group = 2;
        else if (row >= 1 && row <= 3 && col >= 7 && col <= 9) group = 3;

        else if (row >= 4 && row <= 6 && col >= 1 && col <= 3) group = 4;
        else if (row >= 4 && row <= 6 && col >= 4 && col <= 6) group = 5;
        else if (row >= 4 && row <= 6 && col >= 7 && col <= 9) group = 6;

        else if (row >= 7 && row <= 9 && col >= 1 && col <= 3) group = 7;
        else if (row >= 7 && row <= 9 && col >= 4 && col <= 6) group = 8;
        else if (row >= 7 && row <= 9 && col >= 7 && col <= 9) group = 9;

        return group;
    }

    private SolutionStatuses FillItems(out uint counter,List<SolutionItem> model, uint tryCount, uint totelCount)
    {
        model = model.OrderBy(x => x.Id).ToList();

        uint totelCounter = 0;
        uint tryCounter = 0;

        Random rnd = new Random();

    tryAgain:

        foreach (SolutionItem item in model)
            item.Value = 0;

        foreach (SolutionItem item in model)
        {
            byte celVal = (byte)rnd.Next(1, 10);

            while (tryCounter < tryCount)
            {

                #region İlk Satır
                //ilk satır
                if (item.RowNumber == 1)
                {   //ilk kolon ise kontrole gerek yok
                    if (item.ColumnNumber == 1) break;
                    //son kolon ise
                    if (item.ColumnNumber == 9)
                    {
                        celVal = MissingRowNunber(model, item.RowNumber);
                        break;
                    }
                    //aradaki satırlar
                    //if (item.ColumnNumber is > 1 and < 9)
                    //{
                    if (!RowNunbers(model, item.RowNumber).Contains(celVal)) break;

                    //}

                }
                #endregion

                #region Son Satır
                if (item.RowNumber == 9)
                {
                    celVal = MissingRowNunber(model, item.ColumnNumber);
                    if (celVal != 0)
                        break;
                }
                #endregion
                #region Aradaki Satırlar 
                if (item.RowNumber is > 1 and < 9)
                {
                    //ilk kolon ise sadece kolon ve grubu kontrol et, yoksa devam
                    if (item.ColumnNumber == 1 &&
                        !ColumNunbers(model, item.ColumnNumber).Contains(celVal) &&
                        !GrupNunbers(model, item.GroupNumber).Contains(celVal))
                    {
                        break;
                    }
                    //son kolon için tüm yönleri kontorl et
                    if (item.ColumnNumber == 9)
                    {
                        celVal = MissingRowNunber(model, item.RowNumber);
                        break;
                    }
                    //tüm yönleri kontol et
                    if (!RowNunbers(model,item.RowNumber).Contains(celVal) &&
                    !ColumNunbers(model, item.ColumnNumber).Contains(celVal) &&
                    !GrupNunbers(model, item.GroupNumber).Contains(celVal))
                    {
                        break;
                    }

                }
                #endregion

                celVal = (byte)rnd.Next(1, 10);
                //sayaçları arttırır ve tekrar kontrole başla
                totelCount++;
                tryCounter++;

            }

            //limit
            if (totelCounter >= totelCount)
            {
                counter = totelCounter;
                return SolutionStatuses.UnSuccesful;
            }
            //ok
            item.Value = celVal;
           tryCounter++;
            totelCounter++;
            if (tryCounter >= totelCount)
            {
                tryCounter++;
                goto tryAgain;
            }


        }
        counter = totelCounter;
        return SolutionStatuses.Succesful;

      
    
    
    }

    List<byte> RowNunbers(List<SolutionItem> model, byte row)
    {
        List<byte> result = new List<byte>();

        foreach (SolutionItem item in model.Where(t => t.RowNumber == row && t.Value != 0))
            result.Add(item.Value);

        return result;

    }
    List<byte> ColumNunbers(List<SolutionItem> model, byte col)
    {
        List<byte> result = new List<byte>();

        foreach (SolutionItem item in model.Where(t => t.RowNumber == col && t.Value != 0))
            result.Add(item.Value);

        return result;

    }

    List<byte> GrupNunbers(List<SolutionItem> model, byte grp)
    {
        List<byte> result = new List<byte>();

        foreach (SolutionItem item in model.Where(t => t.RowNumber == grp && t.Value != 0))
            result.Add(item.Value);

        return result;

    }
    byte MissingRowNunber(List<SolutionItem> model, byte row)
    {
        //Bir hashset kullanarak dizi içindeki sayıları ekleyin.
        //1'den 9'a kadar olan sayılari içeren başka bir HushSet oluşturun.
        //iki kumenin farknı alarak eksik olan sayıyı bulun.
        HashSet<byte> rowNuns = new(RowNunbers(model, row));
        HashSet<byte> allNums = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        allNums.ExceptWith(rowNuns);
        return allNums.First();
    }
    byte MissingColNunber(List<SolutionItem> model, byte col)
    {

        HashSet<byte> colNuns = new(ColumNunbers(model, col));
        HashSet<byte> allNums = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        allNums.ExceptWith(colNuns);
        if (allNums.Count == 1)
            return allNums.First();
        else
            return 0;
    }
    SolutionStatuses CheckSolution(out string response,List<SolutionItem> model)
    {
        //satır kontrolü tüm satırlarda dön
        for (byte i = 1; i < 10; i++) 
        {
            //satır sayılarını çek
            List<byte> nunbers = RowNunbers(model, i);
            foreach (byte item in nunbers)
            {
                //tekrar eden bir sayı olup olmadığına bak
                int count = nunbers.Where(x => x == item).Count();
                if (count > 1)
                {
                    response = $"Veri doğrulama hatası: Hatalı Satır = {i}, Tekrar eden sayı = {item}, Tekrar sayaç = {count}.";
                    return SolutionStatuses.False;
                }
            }
        }
    

    
        //satır kontrolü tüm satırlarda dön
        for (byte i = 1; i < 10; i++)
        {
            //satır sayılarını çek
            List<byte> nunbers = ColumNunbers(model, i);
            foreach (byte item in nunbers)
            {
                //tekrar eden bir sayı olup olmadığına bak
                int count = nunbers.Where(x => x == item).Count();
                if (count > 1)
                {
                    response = $"Veri doğrulama hatası: Hatalı Satır = {i}, Tekrar eden sayı = {item}, Tekrar sayaç = {count}.";
                    return SolutionStatuses.False;
                }
            }
        }
    
   
        //satır kontrolü tüm satırlarda dön
        for (byte i = 1; i < 10; i++)
        {
            //satır sayılarını çek
            List<byte> nunbers = GrupNunbers(model, i);
            foreach (byte item in nunbers)
            {
                //tekrar eden bir sayı olup olmadığına bak
                int count = nunbers.Where(x => x == item).Count();
                if (count > 1)
                {
                    response = $"Veri doğrulama hatası: Hatalı Satır = {i}, Tekrar eden sayı = {item}, Tekrar sayaç = {count}.";
                    return SolutionStatuses.False;
                }
            }
        }
   
     response = string.Empty;
        return SolutionStatuses.Succesful;
    
    
    }

}
