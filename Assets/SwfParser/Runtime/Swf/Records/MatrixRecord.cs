namespace SwfParserRuntime {

    [System.Serializable]
    public struct MatrixRecord {

        public bool hasScale;
        public byte nScaleBits;
        /// <summary> 缩放x (与矩阵中的 a 一致) </summary>
        public float scaleX;
        /// <summary> 缩放y (与矩阵中的 d 一致) </summary>
        public float scaleY;

        public bool hasRotate;
        public byte nRotateBits;
        /// <summary> 旋转倾斜0 (与矩阵中的 b 一致) </summary>
        public float rotateSkew0;
        /// <summary> 旋转倾斜1 (与矩阵中的 c 一致) </summary>
        public float rotateSkew1;

        public byte nTranslateBits;
        /// <summary> 平移x (除以20后，与矩阵中的 tx 一致) </summary>
        public int translateX;
        /// <summary> 平移y (除以20后，与矩阵中的 ty 一致) </summary>
        public int translateY;

        public MatrixRecord(SwfByteArray bytes) {
            // 缩放
            nScaleBits = 0;
            scaleX = scaleY = 1.0f;
            hasScale = bytes.ReadFlag();
            if (hasScale) {
                nScaleBits = (byte)bytes.ReadUB(5);
                scaleX = bytes.ReadFB(nScaleBits);
                scaleY = bytes.ReadFB(nScaleBits);
            }

            // 旋转倾斜
            nRotateBits = 0;
            rotateSkew0 = rotateSkew1 = 0f;
            hasRotate = bytes.ReadFlag();
            if (hasRotate) {
                nRotateBits = (byte)bytes.ReadUB(5);
                rotateSkew0 = bytes.ReadFB(nRotateBits);
                rotateSkew1 = bytes.ReadFB(nRotateBits);
            }

            // 平移
            nTranslateBits = 0;
            translateX = translateY = 0;
            nTranslateBits = (byte)bytes.ReadUB(5);
            translateX = bytes.ReadSB(nTranslateBits);
            translateY = bytes.ReadSB(nTranslateBits);
        }

        public override string ToString() {
            return $"{nameof(MatrixRecord)}(nScaleBits={nScaleBits}, nRotateBits={nRotateBits}, nTranslateBits={nTranslateBits}, scaleX={scaleX}, rotateSkew0={rotateSkew0}, rotateSkew1={rotateSkew1}, scaleY={scaleY}, translateX={translateX}, translateY={translateY})";
        }

        public Matrix ToMatrix() {
            return new Matrix(
                a: scaleX,
                b: rotateSkew0,
                c: rotateSkew1,
                d: scaleY,
                tx: translateX / 20.0f,
                ty: translateY / 20.0f
            );
        }

    }
}