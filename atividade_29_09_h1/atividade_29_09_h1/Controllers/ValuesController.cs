// Classe Pessoa
public class Pessoa
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }

    // Método para calcular o IMC
    public double CalcularIMC()
    {
        return Peso / (Altura * Altura);
    }
}

// Interface do repositório
public interface IPessoaRepository
{
    void Adicionar(Pessoa pessoa);
    void Atualizar(Pessoa pessoa);
    void Remover(string cpf);
    Pessoa BuscarPorCpf(string cpf);
    List<Pessoa> BuscarTodas();
    List<Pessoa> BuscarPorIMC(double imcMin, double imcMax);
    List<Pessoa> BuscarPorNome(string nome);
}

// Implementação do repositório
public class PessoaRepository : IPessoaRepository
{
    private readonly List<Pessoa> _pessoas;

    public PessoaRepository()
    {
        _pessoas = new List<Pessoa>();
    }

    public void Adicionar(Pessoa pessoa)
    {
        _pessoas.Add(pessoa);
    }

    public void Atualizar(Pessoa pessoa)
    {
        var pessoaExistente = BuscarPorCpf(pessoa.Cpf);
        if (pessoaExistente != null)
        {
            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.Peso = pessoa.Peso;
            pessoaExistente.Altura = pessoa.Altura;
        }
    }

    public void Remover(string cpf)
    {
        var pessoa = BuscarPorCpf(cpf);
        if (pessoa != null)
        {
            _pessoas.Remove(pessoa);
        }
    }

    public Pessoa BuscarPorCpf(string cpf)
    {
        return _pessoas.FirstOrDefault(p => p.Cpf == cpf);
    }

    public List<Pessoa> BuscarTodas()
    {
        return _pessoas;
    }

    public List<Pessoa> BuscarPorIMC(double imcMin, double imcMax)
    {
        return _pessoas.Where(p => p.CalcularIMC() >= imcMin && p.CalcularIMC() <= imcMax).ToList();
    }

    public List<Pessoa> BuscarPorNome(string nome)
    {
        return _pessoas.Where(p => p.Nome.Contains(nome)).ToList();
    }
}

// Controller
public class PessoaController
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoaController(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    public void AdicionarPessoa(Pessoa pessoa)
    {
        _pessoaRepository.Adicionar(pessoa);
    }

    public void AtualizarPessoa(Pessoa pessoa)
    {
        _pessoaRepository.Atualizar(pessoa);
    }

    public void RemoverPessoa(string cpf)
    {
        _pessoaRepository.Remover(cpf);
    }

    public List<Pessoa> BuscarTodasPessoas()
    {
        return _pessoaRepository.BuscarTodas();
    }

    public Pessoa BuscarPessoaPorCpf(string cpf)
    {
        return _pessoaRepository.BuscarPorCpf(cpf);
    }

    public List<Pessoa> BuscarPessoasPorIMC(double imcMin, double imcMax)
    {
        return _pessoaRepository.BuscarPorIMC(imcMin, imcMax);
    }

    public List<Pessoa> BuscarPessoasPorNome(string nome)
    {
        return _pessoaRepository.BuscarPorNome(nome);
    }
}

// Programa principal para executar o CRUD
public partial class Program
{
    public static void Main(string[] args)
    {
        IPessoaRepository pessoaRepository = new PessoaRepository();
        PessoaController pessoaController = new PessoaController(pessoaRepository);

        // Adicionando algumas pessoas
        pessoaController.AdicionarPessoa(new Pessoa { Nome = "Humberto", Cpf = "12345678900", Peso = 70, Altura = 1.75 });
        pessoaController.AdicionarPessoa(new Pessoa { Nome = "Maria", Cpf = "98765432100", Peso = 60, Altura = 1.65 });
        pessoaController.AdicionarPessoa(new Pessoa { Nome = "João", Cpf = "45612378900", Peso = 85, Altura = 1.80 });

        // Buscar todas as pessoas
        Console.WriteLine("Todas as pessoas:");
        var todasPessoas = pessoaController.BuscarTodasPessoas();
        foreach (var pessoa in todasPessoas)
        {
            Console.WriteLine($"Nome: {pessoa.Nome}, CPF: {pessoa.Cpf}, IMC: {pessoa.CalcularIMC():F2}");
        }

        // Buscar pessoas por IMC entre 18 e 24
        Console.WriteLine("\nPessoas com IMC entre 18 e 24:");
        var pessoasIMCBom = pessoaController.BuscarPessoasPorIMC(18, 24);
        foreach (var pessoa in pessoasIMCBom)
        {
            Console.WriteLine($"Nome: {pessoa.Nome}, CPF: {pessoa.Cpf}, IMC: {pessoa.CalcularIMC():F2}");
        }

        // Buscar pessoa por nome
        Console.WriteLine("\nPessoas com nome 'Humberto':");
        var pessoasPorNome = pessoaController.BuscarPessoasPorNome("Humberto");
        foreach (var pessoa in pessoasPorNome)
        {
            Console.WriteLine($"Nome: {pessoa.Nome}, CPF: {pessoa.Cpf}");
        }

        // Atualizar dados de uma pessoa
        var pessoaAtualizar = pessoaController.BuscarPessoaPorCpf("12345678900");
        pessoaAtualizar.Peso = 75;
        pessoaController.AtualizarPessoa(pessoaAtualizar);
        Console.WriteLine("\nDados atualizados de Humberto:");
        var pessoaAtualizada = pessoaController.BuscarPessoaPorCpf("12345678900");
        Console.WriteLine($"Nome: {pessoaAtualizada.Nome}, Peso: {pessoaAtualizada.Peso}, IMC: {pessoaAtualizada.CalcularIMC():F2}");

        // Remover uma pessoa
        pessoaController.RemoverPessoa("98765432100");
        Console.WriteLine("\nMaria foi removida.");
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}

