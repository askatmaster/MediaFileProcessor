namespace MediaFileProcessor.Models.Enums;

/// <summary>
/// Enumeration of different types of help options.
/// </summary>
public enum HelpOptionType
{
    /// <summary>
    /// Print advanced tool options in addition to the basic tool options.
    /// </summary>
    Long, 

    /// <summary>
    /// Print complete list of options, including shared and private options 
    /// for encoders, decoders, demuxers, muxers, filters, etc.
    /// </summary>
    Full, 

    /// <summary>
    /// Print detailed information about the decoder named decoder_name. 
    /// Use the -decoders option to get a list of all decoders.
    /// </summary>
    Decoder, 

    /// <summary>
    /// Print detailed information about the encoder named encoder_name. 
    /// Use the -encoders option to get a list of all encoders.
    /// </summary>
    Encoder, 

    /// <summary>
    /// Print detailed information about the demuxer named demuxer_name. 
    /// Use the -formats option to get a list of all demuxers and muxers.
    /// </summary>
    Demuxer, 

    /// <summary>
    /// Print detailed information about the muxer named muxer_name. 
    /// Use the -formats option to get a list of all muxers and demuxers.
    /// </summary>
    Muxer, 

    /// <summary>
    /// Print detailed information about the filter named filter_name. 
    /// Use the -filters option to get a list of all filters.
    /// </summary>
    Filter, 

    /// <summary>
    /// Print detailed information about the bitstream filter named bitstream_filter_name. 
    /// Use the -bsfs option to get a list of all bitstream filters.
    /// </summary>
    Bsf, 

    /// <summary>
    /// Print detailed information about the protocol named protocol_name. 
    /// Use the -protocols option to get a list of all protocols.
    /// </summary>
    Protocol
}