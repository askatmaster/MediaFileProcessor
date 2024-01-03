namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration of CPU flags used in x86 architecture.
/// </summary>
public enum CpuFlags
{
    /// <summary>
    /// MMX is a SIMD instruction set designed by Intel, introduced in 1997 with their P5-based Pentium line of microprocessors.
    /// </summary>
    MMX = 1,

    /// <summary>
    /// MMXEXT is a SIMD instruction set extension designed by AMD. 
    /// </summary>
    MMXEXT = 2,

    /// <summary>
    /// SSE (Streaming SIMD Extensions) is a SIMD instruction set extension to the x86 architecture, designed by Intel.
    /// </summary>
    SSE = 3,

    /// <summary>
    /// SSE2 is one of the Intel SIMD processor supplementary instruction sets.
    /// </summary>
    SSE2 = 4,

    /// <summary>
    /// SSE2Slow is an extension of the SSE2 instruction set.
    /// </summary>
    SSE2Slow = 5,

    /// <summary>
    /// SSE3 is the third iteration of the SSE instruction set for the IA-32 architecture.
    /// </summary>
    SSE3 = 6,

    /// <summary>
    /// SSE3Slow is an extension of the SSE3 instruction set.
    /// </summary>
    SSE3Slow = 7,

    /// <summary>
    /// SSSE3 adds 16 new instructions to SSE3. 
    /// </summary>
    SSSE3 = 8,

    /// <summary>
    /// Atom is a system on chip platform designed for smartphones and tablet computers, launched by Intel in 2008.
    /// </summary>
    Atom = 9,

    /// <summary>
    /// SSE4.1 is a SIMD instruction set used in Intel processors, introduced in 2006 with their Core micro architecture.
    /// </summary>
    SSE4_1 = 10,

    /// <summary>
    /// SSE4.2, like its predecessor SSE4.1, is a SIMD CPU instruction set used in the Intel Core microarchitecture and AMD’s K10 microarchitecture.
    /// </summary>
    SSE4_2 = 11,

    /// <summary>
    /// AVX (Advanced Vector Extensions) are extensions to the x86 instruction set architecture for microprocessors from Intel and AMD.
    /// </summary>
    AVX = 12,

    /// <summary>
    /// AVX2 extends the AVX instruction set with new integer instructions.
    /// </summary>
    AVX2 = 13,

    /// <summary>
    /// XOP is a SIMD instruction set designed by AMD.
    /// </summary>
    XOP = 14,

    /// <summary>
    /// FMA3 is an instruction set designed by Intel and is used in certain AMD processors. 
    /// </summary>
    FMA3 = 15,

    /// <summary>
    /// FMA4 is an instruction set designed by AMD for their processors.
    /// </summary>
    FMA4 = 16,

    /// <summary>
    /// 3dnow is a SIMD instruction set for the MMX integer instructions in the AMD K6-2 microprocessor.
    /// </summary>
    _3dnow = 17,

    /// <summary>
    /// 3dnowext is an extension of the 3DNow! SIMD instruction set.
    /// </summary>
    _3dnowext = 18,

    /// <summary>
    /// BMI1 (Bit Manipulation Instruction Set 1) is an x86 instruction set that introduces several new instructions that offer new ways of performing bitwise operations.
    /// </summary>
    BMI1 = 19,

    /// <summary>
    /// BMI2 (Bit Manipulation Instruction Set 2) is an extension to the x86 instruction set used by Intel and AMD processors.
    /// </summary>
    BMI2 = 20,

    /// <summary>
    /// CMOV (Conditional MOVe) instruction is a feature of some processors like Intel.
    /// </summary>
    CMOV = 21,
    
    /// <summary>
    /// ARMv5TE is a version in the ARM family of general purpose 32-bit RISC microprocessor architecture.
    /// </summary>
    ARMv5TE = 22,

    /// <summary>
    /// ARMv6 is a group of ARM architecture CPU cores designed and licensed by ARM Holdings.
    /// </summary>
    ARMv6 = 23,

    /// <summary>
    /// ARMv6t2 introduces the Thumb-2 instruction set for better performance.
    /// </summary>
    ARMv6t2 = 24,

    /// <summary>
    /// Vector Floating Point (VFP) is used to improve the performance of floating-point operations.
    /// </summary>
    Vector = 25,

    /// <summary>
    /// Advanced SIMD, also known as Neon, is a combined 64- and 128-bit SIMD instruction set.
    /// </summary>
    Neon = 26,

    /// <summary>
    /// SETEND allows changing of the byte endianness (big endian or little endian).
    /// </summary>
    SETEND = 27,
    
    /// <summary>
    /// ARMv8 introduces the 64-bit instruction set.
    /// </summary>
    ARMv8 = 28,
    
    /// <summary>
    /// Altivec is a SIMD extension created by Motorola for PowerPC microprocessors.
    /// </summary>
    Altivec = 29,

    /// <summary>
    /// Enumeration of CPU flags used in x86 architecture.
    /// </summary>
    VFP = 30,
    
    /// <summary>
    /// The Pentium II is a microprocessor from Intel. It is based on the P6 microarchitecture.
    /// </summary>
    pentium2 = 31,

    /// <summary>
    /// The Pentium III is a microprocessor from Intel that is based on its predecessor, the Pentium II.
    /// </summary>
    pentium3 = 32,

    /// <summary>
    /// The Pentium 4 is a line of single-core central processing units (CPUs) for desktops, laptops and entry-level servers introduced by Intel.
    /// </summary>
    pentium4 = 33,

    /// <summary>
    /// The K6 is a series of microprocessors that were designed by Advanced Micro Devices (AMD). 
    /// </summary>
    k6 = 34,

    /// <summary>
    /// K6-2 is a model in the AMD K6 line of microprocessors.
    /// </summary>
    k62 = 35,

    /// <summary>
    /// The Athlon is a series of 32-bit microprocessors designed and marketed by AMD to compete with Intel's Pentium processors.
    /// </summary>
    athlon = 36,

    /// <summary>
    /// Athlon XP is a family of 32-bit microprocessors designed and manufactured by AMD based on the Athlon microarchitecture.
    /// </summary>
    athlonxp = 37,

    /// <summary>
    /// K8 is a microprocessor architecture designed by AMD as the successor to the K7 architecture.
    /// </summary>
    k8 = 38
}