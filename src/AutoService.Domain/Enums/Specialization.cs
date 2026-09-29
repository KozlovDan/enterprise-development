namespace AutoService.Domain.Enums;

/// <summary>Направление работ и соответствующая специализация механика.</summary>
public enum Specialization
{
    /// <summary>Регламентное техническое обслуживание.</summary>
    Maintenance = 1,

    /// <summary>Диагностика и ремонт двигателя.</summary>
    Engine = 2,

    /// <summary>Обслуживание и ремонт трансмиссии.</summary>
    Transmission = 3,

    /// <summary>Ремонт электрооборудования.</summary>
    Electrical = 4,

    /// <summary>Кузовные и лакокрасочные работы.</summary>
    Body = 5,

    /// <summary>Шиномонтаж и балансировка колёс.</summary>
    Tires = 6
}
