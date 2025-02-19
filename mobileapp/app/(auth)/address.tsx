import { useState } from 'react';
import { View, Text } from 'react-native';
import { Href, router, useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import { ThemedText } from '@/components/ThemedText';
import AuthHeader from '@/components/auth/AuthHeader';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

const loginSchema = z.object({
	cep: z.string().min(1, 'Campo obrigatório!!'),
	logradouro: z.string().min(1, 'Campo obrigatório!!'),
	numero: z.string().min(0),
	complemento: z.string().min(0),
	bairro: z.string().min(1, 'Campo obrigatório!!'),
	cidade: z.string().min(1, 'Campo obrigatório!!'),
	estado: z.string().min(1, 'Campo obrigatório!!'),
});

type LoginFormData = z.infer<typeof loginSchema>;

export default function LoginScreen() {
	const router = useRouter();
	const params = useLocalSearchParams();
	const {
		cep,
		logradouro,
		bairro,
		cidade,
		estado,
	} = params;
	const { control, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			cep: (cep ?? '') as string,
			logradouro: (logradouro ?? '') as string,
			numero: '',
			complemento: '',
			bairro: (bairro ?? '') as string,
			cidade: (cidade ?? '') as string,
			estado: (estado ?? '') as string,
		},
	});

	const [loading, setLoading] = useState(false);

	const onSubmit = async (data: LoginFormData) => {
		try {
			await new Promise(resolve => setTimeout(resolve, 600));

			router.push('/(auth)/register');
		} catch (error: any) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					icon="map-outline"
					title="Casdastrar endereço"
					description="Insira os dados solicitados abaixo para cadastrar seu endereço."
				/>

				<View className="flex flex-col w-full gap-4">
					<Controller
						control={control}
						name="cep"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="CEP"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={true}
								error={!!errors.cep}
								errorMessage={errors.cep?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="logradouro"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Logradouro"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.logradouro}
								errorMessage={errors.logradouro?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="numero"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								keyboardType="numeric"
								returnKeyType="next"
								placeholder="Número"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.numero}
								errorMessage={errors.numero?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="complemento"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Complemento"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.complemento}
								errorMessage={errors.complemento?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="bairro"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Bairro"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={true}
								error={!!errors.bairro}
								errorMessage={errors.bairro?.message}
							/>
						)}
					/>

					<View className="grid grid-cols-2 gap-4">
						<Controller
							control={control}
							name="cidade"
							render={({ field: { onChange, onBlur, value } }) => (
								<TextInput
									returnKeyType="next"
									placeholder="Cidade"
									value={value}
									onChangeText={onChange}
									onBlur={onBlur}
									disabled={true}
									error={!!errors.cidade}
									errorMessage={errors.cidade?.message}
								/>
							)}
						/>
						<Controller
							control={control}
							name="estado"
							render={({ field: { onChange, onBlur, value } }) => (
								<TextInput
									returnKeyType="next"
									placeholder="Estado"
									value={value}
									onChangeText={onChange}
									onBlur={onBlur}
									disabled={true}
									error={!!errors.estado}
									errorMessage={errors.estado?.message}
								/>
							)}
						/>
					</View>

					<Button
						onPress={handleSubmit(onSubmit)}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<View className="flex flex-row items-center gap-2">
							<Text className="text-white">DADOS PESSOAIS</Text>
							<Ionicons name="arrow-forward" color="#ffffff" />
						</View>
					</Button>

					<Button
						onPress={() => router.push('/(auth)/login')}
						className="w-full"
						disabled={loading}
					>
						<ThemedText>LOGIN</ThemedText>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}
